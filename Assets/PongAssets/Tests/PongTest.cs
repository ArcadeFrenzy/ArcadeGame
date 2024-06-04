using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PongTest
{
    [OneTimeSetUp]
    public void SetupTests()
    {
        SceneManager.LoadScene("Pong");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameManagerExistsTest()
    {
        GameObject pgmObj = GameObject.Find("GameManager");
        yield return null;

        Assert.NotNull(pgmObj);
        
        PongGameManager pgm = pgmObj.GetComponent<PongGameManager>();
        Assert.NotNull(pgm);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator Player1ScoredTest()
    {
        GameObject pgmObj = GameObject.Find("GameManager");
        yield return null;

        Assert.NotNull(pgmObj);

        PongGameManager pgm = pgmObj.GetComponent<PongGameManager>();
        Assert.NotNull(pgm);

        pgm.Player1Scored();
        Assert.AreEqual(1, pgm.Player1Score);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator Player2ScoredTest()
    {
        GameObject pgmObj = GameObject.Find("GameManager");
        yield return null;

        Assert.NotNull(pgmObj);

        PongGameManager pgm = pgmObj.GetComponent<PongGameManager>();
        Assert.NotNull(pgm);

        pgm.Player2Scored();
        Assert.AreEqual(1, pgm.Player2Score);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
