using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Julia Bugaj
 * 
 * The TransitionPoint class manages scene transitions triggered by different conditions.
 */
public class TransitionPoint : MonoBehaviour
{
    [SerializeField] string sceneName; /* Name of the target scene. */
    [SerializeField] string sceneNameMiniGame; /* Name of the mini-game scene. */
    [SerializeField] string prefsName; /* PlayerPrefs key for tracking mini-game completion. */
    [SerializeField] string requiredKey; /* First key required to access the scene. */
    [SerializeField] string requiredKey2; /* Second key required to access the scene */
    [SerializeField] string requiredKey3; /* Third key required to access the scene */
    [SerializeField] GameObject obj; /* GameObject to activate/deactivate. */
    [SerializeField] bool stopMusic = false; /* Indicating whether to stop music during the transition. */
    [SerializeField] bool startMusic = true; /* Indicating whether to start music during the transition. */

    /**
     * Method to change the scene, requiring a specific key.
     * It uses the SceneController to handle the transition.
     */
    public void ChangeSceneWithKey()
    {
        SceneController.instance.LoadSceneWithKey(sceneName, requiredKey, obj, stopMusic, startMusic);
    }
    /**
     * Method to change the scene without requiring a key.
     * It uses the SceneController to handle the transition.
     */
    public void ChangeSceneWithoutKey()
    {

        SceneController.instance.LoadSceneWithoutKey(sceneName, stopMusic, startMusic);

    }
    /**
     * Method to change the scene to the endgame scene, requiring multiple keys.
     * It uses the SceneController to handle the transition.
     */
    public void ChangeSceneToEndgame()
    {

        SceneController.instance.LoadEndgame(sceneName, requiredKey, requiredKey2, requiredKey3, obj, stopMusic, startMusic);


    }
    /**
     * Method to change the scene with the possibility of transitioning to a mini-game.
     * It uses the SceneController to handle the transition.
     */
    public void ChangeSceneWithMiniGame()
    {

        SceneController.instance.LoadSceneWithMiniGame(sceneName, sceneNameMiniGame, prefsName, stopMusic, startMusic);


    }

}
