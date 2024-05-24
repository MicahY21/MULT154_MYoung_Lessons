using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;



[System.Serializable]

public class Score
{
    public Score(string n, float t)
    {
        name = n;
        time = t;
    }
    public string name;
    public float time;
}

public class ScoreList : MonoBehaviour
{

    public List<Score> scores = new List<Score>();
    public string fileName;
    public GameObject input;

    public GameObject finalPanel;
    public GameObject scorePanel;

    public GameObject entryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(fileName))
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Create)))
            {
                while (true)
                {
                    try
                    {
                        string name = reader.ReadString();
                        float time = reader.ReadSingle();
                        scores.Add(new Score(name, time));
                    }


                    catch (EndOfStreamException)
                    {
                        break;
                    }



                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewEntry()
    {
        string name = input.GetComponent<TMP_InputField>().text;
        float time = Time.time - GameData.gamePlayStart;
        scores.Add(new Score(name, time));

        foreach(Score score in scores)
        {
            GameObject temp = Instantiate(entryPrefab);
            Transform[] children = temp.GetComponentsInChildren<Transform>();
            children[1].GetComponent<TextMeshProUGUI>().text = score.name;
            children[2].GetComponent<TextMeshProUGUI>().text = score.time.ToString("F2");

            temp.transform.SetParent(scorePanel.transform);
        }


        finalPanel.SetActive(false);
        scorePanel.SetActive(true);
    }

    private void OnDestroy()
    {
        using (BinaryWriter write = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate)))
        {
            foreach(Score score in scores)
            {
                write.Write(score.name);
                write.Write(score.time);
            }
        }
    }

}
