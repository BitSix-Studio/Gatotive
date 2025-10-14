using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Configurações do Jogador")]
    [SerializeField] private JoystickMove joystick; // Referência ao script do Joystick para controlar o jogador
    [SerializeField] private float speedPlayer; // Velocidade de movimento do jogador

    [Header ("Configurações da Câmera do Jogo")]
    [SerializeField] private Camera cam; // variável da camera do jogo
    private float screenZ; // distância da camera em relação ao cenário do jogo
    [SerializeField] private float offSetY; // altura da camera em relação ao jogador
    [SerializeField] private BoxCollider2D worldBounds2d; // variavel do box collider que limita o movimento da camera
    private float halfHeight; // altura até onde a camera pode ir em relação ao Y
    private float halfWidth; // os lados até onde a camera pode ir em relação ao X

    private Rigidbody2D rb2D; // variavel referente ao rigidbody2D do jogador
    private Animator anim; // variavel do componente animator do jogador

    private void Awake()
    {
        cam = Camera.main; // atribui a camera principal da cena para a variavel cam
        rb2D = GetComponent<Rigidbody2D>(); // atribui o componente de corpo rigido do jogador numa variavel do script
        anim = GetComponent<Animator>(); // atribui o componente de animação do jogador numa variavel do script
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenZ = cam.transform.position.z; // pega a posição inicial do eixo Z da camera e atribui na variavel screenZ
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick.joystickVector.y != 0)
        {
            anim.SetBool("IsWalk", true);
            rb2D.linearVelocity = new Vector2(joystick.joystickVector.x * speedPlayer, joystick.joystickVector.y * speedPlayer).normalized;
            anim.SetFloat("X", rb2D.linearVelocity.x);
            anim.SetFloat("Y", rb2D.linearVelocity.y);
        }
        else
        {
            anim.SetBool("IsWalk", false);
            rb2D.linearVelocity = Vector2.zero;
        }
        
    }

    private void LateUpdate()
    {
        EnframeCamera();
    }

    private void EnframeCamera()
    {
        Vector3 relationWithPlayer = new Vector3(transform.position.x, (transform.position.y + offSetY), screenZ);
        cam.transform.position = relationWithPlayer;

        // Calcula o tamanho visível da camera do jogo
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        // Obtém os limites do cenário (do collider)
        Bounds bounds = worldBounds2d.bounds;

        // Calcula as restrições levando em conta o tamanho da câmera
        float minX = bounds.min.x + halfWidth;
        float maxX = bounds.max.x - halfWidth;
        float minY = bounds.min.y + halfHeight;
        float maxY = bounds.max.y - halfHeight;

        // Restringe a posição da câmera
        Vector3 pos = cam.transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        cam.transform.position = new Vector3(pos.x, pos.y, screenZ);
    }
}
