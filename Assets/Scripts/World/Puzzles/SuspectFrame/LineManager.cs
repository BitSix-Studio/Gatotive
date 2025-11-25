using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance;

    public RectTransform lineContainer;
    [SerializeField] private GameObject linePrefab, pinPrefab;

    private List<PaperConnection> connections = new List<PaperConnection>();
    public List<CorrectConnections> correctConnections;


    private RectTransform tempLine;
    private PapelSuspect startPaper;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SuspectFrame.Instance.HideFrame();
    }

    public List<PaperConnection> GetConnections() { return connections; }

    public List<PapelSuspect> GetConnectionsOf(PapelSuspect paper)
    {
        List<PapelSuspect> list = new List<PapelSuspect>();

        foreach (var conn in connections)
        {
            if (conn.paperA == paper)
                list.Add(conn.paperB);

            if (conn.paperB == paper)
                list.Add(conn.paperA);
        }

        return list;
    }

    private void Update()
    {
        // Atualiza todas as linhas existentes
        UpdateAllConnections();
        // Atualiza todos os alfinetes existentes
        UpdateAllPins();
    }

    #region Criação dos desenhos das linhas

    // Começa o desenho da linha
    public void StartLine(PapelSuspect start)
    {
        if (tempLine != null) return;
        if (start.isLocked) return;

        startPaper = start;

        GameObject obj = Instantiate(linePrefab, lineContainer);
        tempLine = obj.GetComponent<RectTransform>();

        UpdateTempLine(start.GetCenterScreen());
    }

    // Cria uma linha temporária enquanto ainda não houve uma conexão feita
    public void UpdateTempLine(Vector2 pointerScreenPos)
    {
        if (tempLine == null) return;

        Vector2 startPos, endPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, startPaper.GetTopScreen(), null, out startPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, pointerScreenPos, null, out endPos);

        PositionLine(tempLine, startPos, endPos);
    }

    // Desenha a linha conectada entre dois papeis
    public void FinalizeLine(PapelSuspect endPaper)
    {
        if (tempLine == null) return;

        // Bloqueia se liberou em cima de papel locked
        if (endPaper != null && endPaper.isLocked)
        {
            Destroy(tempLine.gameObject);
            tempLine = null;
            startPaper = null;
            return;
        }

        // Se soltar fora de qualquer papel ou no mesmo papel, descarta
        if (endPaper == null || endPaper == startPaper)
        {
            Destroy(tempLine.gameObject);
        }
        else
        {
            // checa se já existe conexão
            if (ConnectionExists(startPaper, endPaper))
            {
                // já existe -> descarta a linha temporária
                Destroy(tempLine.gameObject);
            }
            else
            {
                // cria conexão normal
                Vector2 posA = startPaper.GetTopScreen();
                Vector2 posB = endPaper.GetTopScreen();

                Vector2 aLocal, bLocal;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, posA, null, out aLocal);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, posB, null, out bLocal);

                PositionLine(tempLine, aLocal, bLocal);

                connections.Add(new PaperConnection(tempLine, startPaper, endPaper));

                // Verifica se startPaper completou as conexões corretas
                if (PaperHasAllCorrectConnections(startPaper))
                {
                    startPaper.isLocked = true;
                    SpawnPin(startPaper);
                }

                // Verifica se endPaper completou as conexões corretas (caso contrário o jogador pode continuar conectando)
                if (PaperHasAllCorrectConnections(endPaper))
                {
                    endPaper.isLocked = true;
                    SpawnPin(endPaper);
                }
            }
        }

        tempLine = null;
        startPaper = null;
    }

    #endregion

    #region Posiciona os alfinetes nos papeis

    // Faz o alfinete aparecer no papel
    private void SpawnPin(PapelSuspect paper)
    {
        // Já tem um alfinete? Não cria outro
        if (paper.pinInstance != null)
            return;

        // Instancia como filho do container de linhas (ou outro do canvas)
        GameObject pin = Instantiate(pinPrefab, lineContainer);

        // Guarda referência no papel
        paper.pinInstance = pin;

        // Pega posição de topo do papel, em coordenadas de tela
        Vector2 screenPos = paper.GetTopScreen();

        // Converte para coordenada local do canvas
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineContainer,
            screenPos,
            null,
            out localPos
        );

        // Aplica posição
        RectTransform rt = pin.GetComponent<RectTransform>();
        rt.anchoredPosition = localPos;
    }

    // Atualiza a posição de todos os alfinetes
    private void UpdateAllPins()
    {
        foreach (var conn in connections)
        {
            // Para cada papel da conexão, atualiza seu alfinete (se tiver)
            UpdatePinPosition(conn.paperA);
            UpdatePinPosition(conn.paperB);
        }
    }

    // Atualiza a posição de cada alfinete de acordo com o papel em que esta
    private void UpdatePinPosition(PapelSuspect paper)
    {
        if (paper.pinInstance == null)
            return;

        Vector2 screenPos = paper.GetTopScreen();

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineContainer,
            screenPos,
            null,
            out localPos
        );

        RectTransform rt = paper.pinInstance.GetComponent<RectTransform>();
        rt.anchoredPosition = localPos;
    }

    #endregion

    #region Posiciona o desenho da linha criada

    // Atualiza todas as posições das linhas
    private void UpdateAllConnections()
    {
        foreach (var conn in connections)
        {
            Vector2 posA = conn.paperA.GetTopScreen();
            Vector2 posB = conn.paperB.GetTopScreen();

            Vector2 aLocal, bLocal;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, posA, null, out aLocal);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, posB, null, out bLocal);

            PositionLine(conn.line, aLocal, bLocal);
        }
    }

    // Posiciona a linha criada
    private void PositionLine(RectTransform line, Vector2 a, Vector2 b)
    {
        Vector2 center = (a + b) / 2f;
        line.anchoredPosition = center;

        float dist = Vector2.Distance(a, b);
        line.sizeDelta = new Vector2(line.sizeDelta.x, dist);

        Vector2 dir = (b - a).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        line.localRotation = Quaternion.Euler(0, 0, angle);
    }

    #endregion

    // Verifica se a conexão pode ser feita
    private bool ConnectionExists(PapelSuspect a, PapelSuspect b)
    {
        foreach (var conn in connections)
        {
            // Conexão pode ser A-B ou B-A
            if ((conn.paperA == a && conn.paperB == b) ||
                (conn.paperA == b && conn.paperB == a))
            {
                return true;
            }
        }

        return false;
    }

    #region Verifica as conexões corretas

    // Verifica se uma conexão feita está certa
    private bool IsCorrectConnection(PapelSuspect a, PapelSuspect b)
    {
        foreach (var cc in correctConnections)
        {
            if ((cc.paperA == a && cc.paperB == b) ||
                (cc.paperA == b && cc.paperB == a))
            {
                return true;
            }
        }

        return false;
    }

    // Verifica se um papel tem todas as suas conexões corretas e nenhuma errada
    public bool PaperHasAllCorrectConnections(PapelSuspect paper)
    {
        int correctNeeded = 0;
        int correctMade = 0;

        // Conta quantas conexões corretas esse papel deveria ter
        foreach (var cc in correctConnections)
        {
            if (cc.paperA == paper || cc.paperB == paper)
                correctNeeded++;
        }

        // Percorre todas as conexões feitas pelo jogador
        foreach (var conn in connections)
        {
            bool isCorrect = IsCorrectConnection(conn.paperA, conn.paperB);

            // Se é uma conexão ERRADA ligada ao papel — já retorna falso
            if (!isCorrect && (conn.paperA == paper || conn.paperB == paper))
                return false;

            // Conta apenas conexões corretas
            if (isCorrect && (conn.paperA == paper || conn.paperB == paper))
                correctMade++;
        }

        return correctMade == correctNeeded;
    }

    // Verifica se todas as conexões feitas são corretas
    public bool AllConnectionsAreCorrect()
    {
        foreach (var conn in connections)
        {
            if (!IsCorrectConnection(conn.paperA, conn.paperB))
            {
                return false; // achou uma incorreta -> false
            }
        }

        return true;
    }

    // Verifica se o jogador encontrou todas as conexões corretas
    public bool HasAllCorrectConnections()
    {
        // Se existir QUALQUER conexão errada -> já falhou
        foreach (var conn in connections)
        {
            if (!IsCorrectConnection(conn.paperA, conn.paperB))
                return false;
        }

        // Verifica se todas as corretas foram feitas
        foreach (var cc in correctConnections)
        {
            if (!ConnectionExists(cc.paperA, cc.paperB))
                return false;
        }

        return true;
    }

    #endregion

    // Limpa todas as linhas da cena de acordo com as que estão erradas
    public void ClearAllConnections()
    {
        // Cria uma lista para guardar as conexões que devem ser removidas
        List<PaperConnection> toRemove = new List<PaperConnection>();

        foreach (var conn in connections)
        {
            // Se algum dos papéis já completou suas conexões, não remove essa linha
            if (conn.paperA.isLocked || conn.paperB.isLocked)
                continue;
            
            // Caso contrário, essa linha pode ser removida
            if (conn.line != null)
                Destroy(conn.line.gameObject);
            
            // Marca para remover da lista
            toRemove.Add(conn);
        }

        // Remove da lista principal
        foreach (var conn in toRemove)
        {
            connections.Remove(conn);
        }
    }
}
