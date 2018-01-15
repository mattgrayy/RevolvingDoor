using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Level
{
    LEVEL_LOBBY = 0,
    LEVEL_1 = 1,
    LEVEL_2 = 2
}

public class LevelManager : MonoBehaviour {

    public static LevelManager m_instance;
    [SerializeField] Image transitionImage;

    bool transitioning = false;
    float transitionTimer = 0;
    Level targetLevel = 0;


    private void Start()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (transitioning)
        {
            if (transitionTimer < 1)
            {
                transitionTimer += Time.deltaTime;
                transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, transitionTimer);

                if (transitionTimer >= 1)
                    SceneManager.LoadSceneAsync((int)targetLevel, LoadSceneMode.Single);
            }
            else if (transitionTimer < 2)
            {
                transitionTimer += Time.deltaTime;
                transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1 - (transitionTimer - 1));

                if (transitionTimer >= 2)
                {
                    transitionTimer = 0;
                    transitioning = false;
                    transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 0);
                }
            }
        }
    }

    public void loadLevel(Level _levelToLoad)
    {
        targetLevel = _levelToLoad;
        transitioning = true;
    }
}
