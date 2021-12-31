using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PAL_Task : MonoBehaviour
{
    //Encoding > Recall > Encoding > Recall
    //Novel > Same > Novel > Same
    public string[] NameList;
    public Sprite[] FaceList;
    public GameObject FaceImage;
    public Text NameText;
    public GameObject InputField;
    public GameObject DoneText;
    public bool boolDoneText;
    public int zeroIndex;
    public int PALIndex;
    public int Count; //One Block End 확인용
    public float time;
    public float timeForDoneText;
    public GameObject Blank;

    public bool IsEncoding;
    public bool IsRecalling;
    public bool IsNovel;
    public bool IsSame;
    public bool IsBaseline;


    void Start()
    {
        InputField.SetActive(false);
        DoneText.SetActive(false);
        zeroIndex = 0;
        PALIndex = 1;
        Count = 1;
        time = 0;
        timeForDoneText = 0;
        Blank.SetActive(false);
        boolDoneText = false;

        IsEncoding = false;
        IsRecalling = false;
        IsNovel = false;
        IsSame = false;
        IsBaseline = false;
    }

    void Update()
    {
        if (boolDoneText == true)
        {
            timeForDoneText += Time.deltaTime;
            if (timeForDoneText > 1)
            {
                DoneText.SetActive(false);
                timeForDoneText = 0;
                boolDoneText = false;
            }
        }

        time += Time.deltaTime;

        //Baseline
        if (time > 0 && time < 25f)
        {
            FaceImage.GetComponent<Image>().sprite = FaceList[zeroIndex];
            NameText.text = NameList[zeroIndex];
            IsBaseline = true;
        }

        //Encoding : BlockNum 1,2_PAL Task
        if (time > 25 && time < 29.5f)
        {
            FaceImage.GetComponent<Image>().sprite = FaceList[PALIndex];
            NameText.text = NameList[PALIndex];
            IsBaseline = false;
        }

        //Rest
        if (time > 29.5f && time < 32.5f)
        {
            FaceImage.GetComponent<Image>().sprite = FaceList[zeroIndex];
            NameText.text = NameList[zeroIndex];
            IsBaseline = true;
        }

        if (time > 32.5f)
        {
            PALIndex += 1;
            Count += 1;
            time = 25f;

            //한 블럭 완료시, 25초간 rest
            if (Count % 7 == 1) { time = 0; }

            //Task종료
            if (Count > 112) { Blank.SetActive(true); }
        }

        //Encoding
        if (Count >= 1 && Count <= 28)
        {
            InputField.SetActive(false);
            IsEncoding = true;
            IsRecalling = false;
        }
        if (Count >= 57 && Count <= 84)
        {
            InputField.SetActive(false);
            IsEncoding = true;
            IsRecalling = false;
        }

        //Recalling
        if (Count > 28 && Count < 57)
        {
            InputField.SetActive(true);
            IsRecalling = true;
            IsEncoding = false;
        }
        if (Count > 84 && Count < 112)
        {
            IsRecalling = true;
            IsEncoding = false;
            InputField.SetActive(true);
        }

        //IsNovel
        if (Count == 1 || Count == 2 || Count == 3 || Count == 4 || Count == 5 || Count == 6 || Count == 7 ||
            Count == 15 || Count == 16 || Count == 17 || Count == 18 || Count == 19 || Count == 20 || Count == 21 ||
            Count == 29 || Count == 30 || Count == 31 || Count == 32 || Count == 33 || Count == 34 || Count == 35 ||
            Count == 43 || Count == 44 || Count == 45 || Count == 46 || Count == 47 || Count == 48 || Count == 49 ||
            Count == 57 || Count == 58 || Count == 59 || Count == 60 || Count == 61 || Count == 62 || Count == 63 ||
            Count == 71 || Count == 72 || Count == 73 || Count == 74 || Count == 75 || Count == 76 || Count == 77 ||
            Count == 85 || Count == 86 || Count == 87 || Count == 88 || Count == 89 || Count == 90 || Count == 91 ||
            Count == 99 || Count == 100 || Count == 101 || Count == 102 || Count == 103 || Count == 104 || Count == 105)
        {
            IsNovel = true;
            IsSame = false;
        }

        //IsSame
        if (Count == 8 || Count == 9 || Count == 10 || Count == 11 || Count == 12 || Count == 13 || Count == 14 ||
            Count == 22 || Count == 23 || Count == 24 || Count == 25 || Count == 26 || Count == 27 || Count == 28 ||
            Count == 36 || Count == 37 || Count == 38 || Count == 39 || Count == 40 || Count == 41 || Count == 42 ||
            Count == 50 || Count == 51 || Count == 52 || Count == 53 || Count == 54 || Count == 55 || Count == 56 ||
            Count == 64 || Count == 65 || Count == 66 || Count == 67 || Count == 68 || Count == 69 || Count == 70 ||
            Count == 78 || Count == 79 || Count == 80 || Count == 81 || Count == 82 || Count == 83 || Count == 84 ||
            Count == 92 || Count == 93 || Count == 94 || Count == 95 || Count == 96 || Count == 97 || Count == 98 ||
            Count == 106 || Count == 107 || Count == 108 || Count == 109 || Count == 110 || Count == 111 || Count == 112)
        {
            IsSame = true;
            IsNovel = false;
        }
    }
    public void InputField_()
    {
        boolDoneText = true;
        DoneText.SetActive(true);
    }
}
