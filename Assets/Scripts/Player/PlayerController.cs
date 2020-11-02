using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnPlayerDead;
    public static Action OnPowerActivated;

    [Header("References")]
    [SerializeField, Tooltip("Reference to the players model.")]
    private GameObject m_playerModelReference = null;
    [SerializeField, Tooltip("Reference to the death particle effect.")]
    private ParticleSystem m_deathParticleEffectReference = null;
    [SerializeField, Tooltip("Reference to the trail particle effect.")]
    private ParticleSystem m_trailParticleEffectReference = null;

    [Header("Players parameters")]
    [SerializeField, Tooltip("Value in degrees that indicates the starting point of the player. 0° corresponding to the top of the circle.")]
    private float m_playerStartPositionAngle = 180;
    [SerializeField, Tooltip("The movement speed of the player.")]
    private float m_movementSpeed = 3f;


    //private variable used to store references and control the visual feedback of the player's model
    private MeshRenderer m_playerModelMeshRenderer;
    private ParticleSystem.MainModule m_PSmainModuleReference;
    private Renderer m_playerSkinRendererBuffer;

    //private variables to control the state and the position of the player
    private Vector3 m_desiredPlayerPosition;
    private Vector3 m_startPosition;            //the position of the start location of the level
    private Vector3 m_movingCursorPosition;     //variable used to store the position of the cursor when the player touches the screen (values in pixels!)
    private float m_angle;                      //variable used to calculate the position of the player around the circle
    private float m_levelRadius;
    private float m_movementMagnitude;
    private bool m_canMove;
    private bool m_isAlive;


    private void OnEnable()
    {
        //Gameflow
        GameManager.OnStartGame += Enableplayer;
        GameManager.OnTitleScreen += ResetPlayer;
        GameManager.OnPlayerRevived += RevivePlayer;
        GameManager.OnSkinScreen += DeactivateTrailParticleEffect;

        GameManager.OnLevelRadiusSent += GetLevelRadius;
        GameManager.OnStartPositionSent += GetStartPosition;

        //Controls
        Controller.OnTapBegin += GetStartMovingPosition;
        Controller.OnHold += MovePlayer;

        //Gameplay
        CollectiblePower.OnCollectiblePowerCollected += ApplyBonus;
        ScoreManager.OnBonusThresholdTriggered += ApplyBonus;

        //Cosmetics
        SkinManager.OnApplySkin += ApplySkin;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= Enableplayer;
        GameManager.OnTitleScreen -= ResetPlayer;
        GameManager.OnPlayerRevived -= RevivePlayer;
        GameManager.OnSkinScreen -= DeactivateTrailParticleEffect;

        GameManager.OnLevelRadiusSent -= GetLevelRadius;
        GameManager.OnStartPositionSent -= GetStartPosition;

        Controller.OnTapBegin -= GetStartMovingPosition;
        Controller.OnHold -= MovePlayer;

        CollectiblePower.OnCollectiblePowerCollected -= ApplyBonus;
        ScoreManager.OnBonusThresholdTriggered -= ApplyBonus;

        SkinManager.OnApplySkin -= ApplySkin;
    }

    private void Start()
    {
        //getting references and checking if parameters are not null
        if (m_playerModelReference == null)
            Debug.LogError("The player model gameobject is missing!", gameObject);
        else
        {
            m_playerSkinRendererBuffer = m_playerModelReference.GetComponent<Renderer>();
            m_playerModelMeshRenderer = m_playerModelReference.GetComponent<MeshRenderer>();
        }

        if (m_playerSkinRendererBuffer == null)
            Debug.LogError("The renderer component is null!",gameObject);
        if (m_playerModelMeshRenderer == null)
            Debug.LogError("The mesh renderer component is null!", gameObject);


        if (m_trailParticleEffectReference == null)
            Debug.LogError("The trail particle effect component is missing!",gameObject);
        if (m_deathParticleEffectReference == null)
            Debug.LogError("The death particle effect component is missing!",gameObject);

        m_PSmainModuleReference = m_trailParticleEffectReference.main;

        InitPlayer();
    }


    void Update()
    {
        if (m_canMove)
            MovePlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameTags.OBSTACLE_TAG) && m_isAlive)
        {
            Kill();
            BroadcastOnPlayerDead();
        }
    }

    private void InitPlayer()
    {
        m_canMove = false;
        m_isAlive = true;
        m_trailParticleEffectReference.Play();
    }

    private void Enableplayer()
    {
        m_isAlive = true;
        m_canMove = true;
        m_playerModelMeshRenderer.enabled = true;

        ActivateTrailParticleEffect();
    }

    private void ResetPlayer()
    {
        m_canMove = false;
        m_isAlive = true;
        m_playerModelMeshRenderer.enabled = true;

        ActivateTrailParticleEffect();
        ResetPlayerPosition();
    }

    private void ActivateTrailParticleEffect()
    {
        m_trailParticleEffectReference.Play();
    }

    private void DeactivateTrailParticleEffect()
    {
        m_trailParticleEffectReference.Stop();
        m_trailParticleEffectReference.Clear();
    }

    private void Kill()
    {
        m_isAlive = false;
        m_canMove = false;
        m_playerModelMeshRenderer.enabled = false;
        m_deathParticleEffectReference.Play();

        DeactivateTrailParticleEffect();
    }

    private void ApplySkin(Material skinMaterial)
    {
        if (m_playerSkinRendererBuffer != null)
        {
            m_playerSkinRendererBuffer.material = skinMaterial;
        }
        if (m_trailParticleEffectReference != null)
        {
            m_PSmainModuleReference.startColor = skinMaterial.GetColor("_EmissionColor");
        }
    }


    private void RevivePlayer()
    {
        m_isAlive = true;
        m_canMove = true;
        m_playerModelMeshRenderer.enabled = true;

        ActivateTrailParticleEffect();
        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        m_angle = m_playerStartPositionAngle;
        MovePlayer();
    }

    private void ApplyBonus()
    {
        BroadcastOnPowerActivated();
    }

    private void GetLevelRadius(float levelRadius)
    {
        m_levelRadius = levelRadius;
    }

    private void GetStartPosition(Vector3 startPosition)
    {
        m_startPosition = startPosition;
    }

    /// <summary>
    /// Moves the player around a circle defined by a radius
    /// </summary>
    private void MovePlayer()
    {
        m_desiredPlayerPosition.x = m_levelRadius * Mathf.Sin(m_angle * Mathf.Deg2Rad) + m_startPosition.x;
        m_desiredPlayerPosition.y = m_levelRadius * Mathf.Cos(m_angle * Mathf.Deg2Rad) + m_startPosition.y;
        m_desiredPlayerPosition.z = m_startPosition.z;
        transform.position = m_desiredPlayerPosition;
    }

    private void GetStartMovingPosition(Vector3 cursorStartPosition)
    {
        m_movingCursorPosition = cursorStartPosition;
    }

    private void MovePlayer(Vector3 tapHoldPosition)
    {
        m_movementMagnitude = m_movingCursorPosition.x - tapHoldPosition.x;

        if (m_movementMagnitude < 0.0f)
        {
            //moving to the left
            m_angle -= m_movementSpeed * -m_movementMagnitude / 10;
        }
        else if (m_movementMagnitude > 0.0f)
        {
            //moving to the right
            m_angle += m_movementSpeed * m_movementMagnitude / 10;
        }

        m_movingCursorPosition = tapHoldPosition;
    }

    #region Broadcasters
    private void BroadcastOnPlayerDead()
    {
        if (OnPlayerDead != null)
            OnPlayerDead();
    }

    private void BroadcastOnPowerActivated()
    {
        if (OnPowerActivated != null)
            OnPowerActivated();
    }
    #endregion
}
