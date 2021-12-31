using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileWriter : MonoBehaviour
{
    public float realTime;
    public GameObject GameManager;
    public int participantNum;                     // 피험자 번호
    public int trialNum;                            // trial 번호

    // 저장 경로는 바탕화면 (Desktop)
    // 바탕화면에 Exp 폴더 생성 후, Participant's identity에 맞게 분류.
    [Header("Directory (root : Desktop")]
    private string path;

    Stream fileStream_Tracking;                 // about Head rotation + Eye movement

    StreamWriter fileWriter_Tracking;

    string fileName_Total_Tracking;

    // 파일 입력이 한번이라도 이루어졌는가 = column 작성 되었는가?
    bool initWriting_Tracking = false;

    void Awake()
    {
        //Environment.SpecialFolder.Desktop : 바탕화면 경로
        //저장폴더 경로지정
        path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\PALTask_Data\\" + participantNum.ToString() + "\\" + trialNum.ToString() + "\\";

        MakeDataDir_Tracking();
    }
    void Start()
    {
        realTime = 0;
        GameManager = GameObject.Find("GameManager");
    }

    void Update()
    {
        realTime += Time.deltaTime;
        //realName, inputName, Correct
        WriteTracking(GameManager.GetComponent<PAL_Task>().IsEncoding, GameManager.GetComponent<PAL_Task>().IsRecalling, 
        GameManager.GetComponent<PAL_Task>().IsNovel, GameManager.GetComponent<PAL_Task>().IsSame, GameManager.GetComponent<PAL_Task>().IsBaseline, realTime);
    }

    public void MakeDataDir_Tracking()
    {
        // 'path = 바탕화면 + 사용자 정의 경로 (실험 데이터 폴더) + 피험자 별 폴더' 디렉토리 생성
        // path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + path;
        Directory.CreateDirectory(path);

        string fileName_Tracking = "PAL_Task_Data";

        string format = ".csv";
        string nowTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        // Dynamic blurring 적용 여부에 따라 general data, fms data 파일 명명
        fileName_Total_Tracking = path + fileName_Tracking + "(" + nowTime + ")" + format;
        // 정의된 실험 데이터 폴더에 데이터 파일을 생성하고, 파일 쓰기를 위한 스트림 인스턴스 생성
        fileStream_Tracking = new FileStream(fileName_Total_Tracking, FileMode.Create, FileAccess.ReadWrite);
        fileWriter_Tracking = new StreamWriter(fileStream_Tracking);
    }

    public bool WriteTracking(bool IsEncoding, bool IsRecalling, bool IsNovel, bool IsSame, bool IsBaseline, float realTime)
    {
        // 스트림에 write를 할 수 있는 상태인지 확인
        if (fileStream_Tracking.CanWrite)
        {
            try
            {
                // 최초 general data 기록시, 컬럼 정보 삽입
                if (initWriting_Tracking != true)
                {
                    string columnInfo = "IsEncoding, IsRecalling, IsNovel, IsSame, IsBaseline, realTime";
                    fileWriter_Tracking.WriteLine(columnInfo);
                    fileWriter_Tracking.Flush();
                    initWriting_Tracking = true;
                }

                fileWriter_Tracking = new StreamWriter(fileStream_Tracking);
                string inputLine = "";

                inputLine = IsEncoding + "," + IsRecalling + "," + IsNovel + "," + IsSame + "," + IsBaseline + "," + realTime;

                fileWriter_Tracking.WriteLine(inputLine);
                fileWriter_Tracking.Flush();
            }
            catch (System.Exception e)
            {
                Debug.LogAssertion("경고! 데이터 디렉토리와 스트림이 확인되었으나, 기록할 수 없습니다. - " + e);
                return false;
            }
            return true;
        }
        return false;
    }

    void OnApplicationQuit()
    {
        if (fileStream_Tracking.CanRead) fileStream_Tracking.Close();
    }
}