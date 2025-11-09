using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ballCountUI;
    [SerializeField]
    TextMeshProUGUI ballNameUI;

    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBallCount();
        UpdateBallName();
    }

    private void UpdateBallCount() 
    {
        ballCountUI.text = BallData.GetInstance().GetNumBounces().ToString();
    }

    private void UpdateBallName()
    {
        ballNameUI.text = BallData.GetInstance().GetNameBall();
    }

    public void NameToJSONData(TextMeshProUGUI name)
    {
        BallData.GetInstance().SetNameBall(name.text);
    }

    public void PauseBallSimulation()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BallData.GetInstance().SetNumBounces(BallData.GetInstance().GetNumBounces() + 1);
        rb.AddForce(new Vector2(30f, 30f));
    }
}
