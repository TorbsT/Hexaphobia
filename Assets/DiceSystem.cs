using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DiceSystem : MonoBehaviour
{
    public static DiceSystem Instance { get; private set; }

    public UpgradePopup upgradePopup;
    public Image loadingScreen;
    public Image timerbar;
    public GameObject deadScreen;
    public GameObject wonScreen;
    public GameObject pauseScreen;
    public TextMeshProUGUI loadingTitle;
    public TextMeshProUGUI loadingDesc;
    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI wonScoreTxt;

    public HashSet<Die> dice = new();

    [Range(5f, 120f)] public float upgradeSpawnInterval = 45f;
    [Range(0f, 10f)] public float defaultSpeed = 1f;
    [Range(0f, 10f)] public float rerollSpeed = 10f;
    [Range(0f, 10f)] public float deacceleration = 1f;
    [Range(0f, 1f)] public float delay = 1f;
    [Range(0f, 10f)] public float timeAddedOnRoll = 5f;
    [Range(0f, 1f)] public float fadingSpeed = 0.1f;
    [Range(1f, 10f)] public float leadSpeedMultiplier;
    public AnimationCurve eyeSpeedCurve;

    public float time = 0f;
    public float endTime = 30f;
    public int score;
    public bool started;
    public float timeSinceLast;
    public bool fading;
    public float speed;
    public bool done;
    public int level = 0;
    public float timerScale = 1f;
    public float diceMass = 1f;
    public float timeSinceUpgradeSpawn;
    public bool leadSkin;

    private void Awake()
    {
        Instance = this;
        loadingScreen.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(StartShit), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;
        if (fading)
        {
            Color c = loadingScreen.color;
            c.a -= fadingSpeed * Time.deltaTime;
            if (c.a <= 0f)
            {
                fading = false;
                started = true;
                time = 0f;
                loadingScreen.gameObject.SetActive(false);
            }
            loadingScreen.color = c;
            Color textColor = new Color(loadingTitle.color.r, loadingTitle.color.g, loadingTitle.color.b, c.a);
            loadingTitle.color = textColor;
            loadingDesc.color = textColor;
        } else
        {
            time += Time.deltaTime*timerScale;
            speed = Mathf.Max(defaultSpeed, speed - deacceleration * Time.deltaTime);

            if (started)
            {
                timerbar.gameObject.SetActive(true);
                timerbar.fillAmount = time / endTime;
                timerTxt.text = "Survive for another "+ Mathf.CeilToInt((endTime - time)/1f) + " seconds";
                
                timeSinceUpgradeSpawn += Time.deltaTime;
                if (timeSinceUpgradeSpawn > upgradeSpawnInterval)
                {
                    timeSinceUpgradeSpawn = 0f;
                    Spawner.Instance.SpawnUpgrade();
                }

                if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
            } else
            {
                timerbar.gameObject.SetActive(false);
            }


            if (time >= endTime)
            {
                Survived();
            }

            if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale != 0f)
            {
                Cam.Instance.animator.SetTrigger("shake");
                Spawner.Instance.Spawn();

                level++;

                    foreach (Die die in dice)
                {
                    die.Flee();
                    int eyes = Random.Range(1, 7);
                    die.Set(eyes);
                }

                speed = rerollSpeed;
                endTime += timeAddedOnRoll;
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetShit();
        }
        */
    }
    public void UpgradePopup(Upgrade upgrade)
    {
        if (upgrade.id == "lead")
        {
            defaultSpeed /= leadSpeedMultiplier;
            rerollSpeed /= leadSpeedMultiplier;
            diceMass += 100f;
            leadSkin = true;
            foreach (Die die in dice)
            {
                die.rb.mass = diceMass;
            }
        }
        else if (upgrade.id == "speedtime")
        {
            timerScale *= 3f/2f;
        }
        else if (upgrade.id == "slowtime")
        {
            timerScale *= 2f / 3f;
        } else if (upgrade.id == "points")
        {
            Score(50);
            defaultSpeed *= 1.1f;
            rerollSpeed *= 1.1f;
        }

        upgradePopup.title = upgrade.title;
        upgradePopup.desc = upgrade.desc;
        upgradePopup.gameObject.SetActive(false);
        upgradePopup.gameObject.SetActive(true);
    }
    private void Survived()
    {
        done = true;
        Time.timeScale = 0f;
        wonScreen.SetActive(true);
        wonScoreTxt.text = "Score: " + score;
    }
    public void Died()
    {
        done = true;
        Score(-score);
        Time.timeScale = 0f;
        deadScreen.SetActive(true);
        Player.Instance.Died();
    }
    public void TogglePause()
    {
        Time.timeScale = 1f - Time.timeScale;
        bool paused = Time.timeScale == 0f;
        pauseScreen.SetActive(paused);
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Score(int points)
    {
        score += points;
        scoreTxt.text = "Score: " + score;
    }
    public void SetShit()
    {
        /*
        ICollection<Die> rawDies = FindObjectsOfType<Die>();
        List<Die> dies = new();
        foreach (Die die in rawDies)
        {
            dies.Insert(Random.Range(0, dies.Count + 1), die);
        }
        int i = 0;
        foreach (Die die in dies)
        {
            die.Set(1);
            i++;
        }
        timeSinceLast = 0f;
        */
    }
    void StartShit()
    {
        fading = true;
    }
}
