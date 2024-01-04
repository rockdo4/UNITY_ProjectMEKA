using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTimer : MonoBehaviour
{
    [Range(1, 100)]
    public int fFont_Size;

    private static float totalTimer;
    private static bool activeTotalTimer;

    private List<string> waveTimer;
    private bool activeWaveTimer;

    private void Start()
    {
        totalTimer = 0f;
        activeTotalTimer = false;

        waveTimer = new List<string>();
        activeWaveTimer = false;
    }
    private void Update()
    {
        UpdateTimer();
        ActiveTimer();
    }
    private void UpdateTimer()
    {
        totalTimer += Time.deltaTime;
    }
    private void ActiveTimer()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            activeTotalTimer = !activeTotalTimer;
        }
    }
    public void AddStartWave(string name)
    {
        waveTimer.Add($"{name}/{totalTimer:F2}�� ����");
    }

    public void AddEndWave(string name)
    {
        waveTimer.Add($"{name}/{totalTimer:F2}�� ��");
    }

    void OnGUI()
    {
        if (!activeTotalTimer) return;

        float offset = 40f;

        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(Screen.safeArea.x, Screen.safeArea.y, w, h * 0.02f);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / fFont_Size;

        GUI.Label(rect, totalTimer.ToString("F2") + "��", style);

        for (int i = 0; i < waveTimer.Count; i++)
        {
            //float timeValue = waveTimer[i];
            //string formattedTime = timeValue.ToString("F2") + "��"; // �ܼ� float ���� ���ڿ��� ��ȯ�ϰ� '��'�� �߰�

            GUI.Label(new Rect(Screen.safeArea.x, Screen.safeArea.y + offset, w, h * 0.02f), waveTimer[i], style); // Label�� �ð� ǥ��
            offset += 40; // ���� Label ��ġ ����
        }
    }
}
