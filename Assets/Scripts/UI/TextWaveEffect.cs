using TMPro;
using UnityEngine;

public class TextWaveEffect : MonoBehaviour
{
    [Header("Configurações de Onda")]
    [SerializeField] private float amplitude = 5f;     // Altura da onda
    [SerializeField] private float frequency = 8f;     // Velocidade da oscilação
    [SerializeField] private float letterOffset = 0.25f; // Tempo de mudança entre letras

    [Header("Tremor Aleatório (opcional)")]
    [SerializeField] private bool useRandomTremor = false;
    [SerializeField] private float tremorAmount = 1.5f; // Força do tremor

    private TextMeshProUGUI textMesh;
    private TMP_TextInfo textInfo;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Garante que o TMP atualize o mesh antes de alterar os vértices
        textMesh.ForceMeshUpdate();

        textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;

            // Calcula a variação senoidal com base no índice da letra
            float wave = Mathf.Sin(Time.time * frequency + i * letterOffset) * amplitude;

            Vector3 offset = new Vector3(0, wave, 0);

            // Adiciona tremor aleatório (opcional)
            if (useRandomTremor)
            {
                float tremorX = (Mathf.PerlinNoise(i, Time.time * frequency) - 0.5f) * tremorAmount;
                float tremorY = (Mathf.PerlinNoise(Time.time * frequency, i) - 0.5f) * tremorAmount;
                offset += new Vector3(tremorX, tremorY, 0);
            }

            // Move os quatro vértices do caractere
            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        // Atualiza o mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
