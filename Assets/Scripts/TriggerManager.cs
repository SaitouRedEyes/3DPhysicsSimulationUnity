using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class TriggerManager : MonoBehaviour
{
    private List<GameObject> triggers = new List<GameObject>();
    private List<Text> textTriggers = new List<Text>();
    private Path path;
    private int numberOfTriggers, currentTriggersCount;
    private float textTriggerPositionOffset = 0;

    public GameObject panel, textTriggerPrefab;

    void Update()
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            if (triggers[i].GetComponentInChildren<Trigger>().Collided)
            {
                textTriggers[i].gameObject.SetActive(true);
                textTriggers[i].text = "Chegou a " + triggers[i].name + "M em aproximadamente " + triggers[i].GetComponentInChildren<Trigger>().ColliderTime + " S";
            }
            else textTriggers[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Update the path triggers in function of the ui input field.
    /// </summary>
    /// <param name="inputFieldPathMaxDistance"> input field path max distance </param>
    public void UpdateTriggers(InputField inputFieldPathMaxDistance)
    {
        if (path == null) path = (Path)GameObject.FindObjectOfType(typeof(Path));

        numberOfTriggers = !inputFieldPathMaxDistance.text.Equals("") ? int.Parse(inputFieldPathMaxDistance.text) / 10 : 0;
        currentTriggersCount = triggers.Count;

        //removing triggers
        if (currentTriggersCount > numberOfTriggers)
        {
            for (int i = currentTriggersCount - 1; i >= 0; i--)
            {
                if (i >= numberOfTriggers)
                {
                    Destroy(triggers[i]);
                    triggers.RemoveAt(i);

                    Destroy(textTriggers[i].gameObject);
                    textTriggers.RemoveAt(i);
                    textTriggerPositionOffset += 0.04f;
                }
                else RepositioningTriggers(i);
            }
        }
        
        //adding triggers
        if (currentTriggersCount <= numberOfTriggers)
        {
            for (int i = 0; i < numberOfTriggers; i++)
            {
                if (i >= currentTriggersCount)
                {
                    triggers.Add((GameObject)Instantiate(Resources.Load("Prefabs/Trigger")));
                    triggers[i].name = (((i + 1) * 10)).ToString();

                    ConfigureTextTrigger();
                }

                RepositioningTriggers(i);
                
                foreach (Transform child in triggers[i].transform)
                {
                    if (child.tag.Equals("Trigger")) child.gameObject.AddComponent<Trigger>();
                    else child.GetComponent<TextMesh>().text = ((i + 1) * 10).ToString() + "m";
                }
            }
        }
    }

    /// <summary>
    /// Configuring the texts triggers
    /// </summary>
    private void ConfigureTextTrigger()
    {
        GameObject textTrigger = (GameObject)Instantiate(textTriggerPrefab, textTriggerPrefab.transform.position, Quaternion.identity);

        textTrigger.transform.SetParent(panel.transform);

        RectTransform textTriggerRT = textTrigger.GetComponent<RectTransform>();
        textTrigger.name = "TextTrigger";        
        textTriggerRT.anchorMin = new Vector2(textTriggerRT.anchorMin.x, textTriggerRT.anchorMin.y + textTriggerPositionOffset);
        textTriggerRT.anchorMax = new Vector2(textTriggerRT.anchorMax.x, textTriggerRT.anchorMax.y + textTriggerPositionOffset);
        textTriggerRT.offsetMin = Vector2.zero;
        textTriggerRT.offsetMax = Vector2.zero;
        textTrigger.transform.localScale = Vector3.one;
        textTrigger.AddComponent<LayoutElement>();
        textTrigger.SetActive(false);

        textTriggers.Add(textTrigger.GetComponent<Text>());

        textTriggerPositionOffset -= 0.04f;
    }

    /// <summary>
    /// Repositioning triggers in function of the new path width
    /// </summary>
    /// <param name="i"> index of array</param>
    private void RepositioningTriggers(int i)
    {
        triggers[i].transform.position = new Vector3((path.transform.localPosition.x - (float.Parse(path.Width) / 2) + (i + 1) * 10),
                                                      triggers[i].transform.position.y,
                                                      triggers[i].transform.position.z);
    }
}