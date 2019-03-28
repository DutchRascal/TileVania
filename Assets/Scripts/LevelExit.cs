using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    int numberOfScenes;

    Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        numberOfScenes = SceneManager.sceneCountInBuildSettings;

    }

    private void OnTriggerEnter2D(Collider2D otherCollision)
    {
        Destroy(otherCollision.gameObject);
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        myAnimator.SetBool("levelExit", true);
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        if (currentSceneIndex + 1 < numberOfScenes)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
