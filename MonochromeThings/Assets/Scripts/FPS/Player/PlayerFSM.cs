using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public enum PlayerFSMEnum
    {
        IDLE = 0,
        MOVE,
        JUMP,
        Attack,
        DASH,
        WALL_JUMP,

        NONE
    }

    /// <summary>
    /// FSM 노드 추상클래스 
    /// </summary>
    public abstract class State_Node
    {
        public List<PlayerFSMEnum> targetStates;

        protected static Dictionary<PlayerFSMEnum, State_Node> currentStates;

        protected PlayerFSMEnum stateType;
        public PlayerFSMEnum StateType
        {
            get { return stateType; }
        }

        protected KeyCode key;
        public KeyCode Key
        {
            get { return key; }
            set { key = value; }
        }

        public  State_Node()
        {
            Constructor();
        }

        protected PlayerController playerMovement;
        
        protected virtual void Constructor()
        {
            if (currentStates == null)
            {
                currentStates = new Dictionary<PlayerFSMEnum, State_Node>();
            }

            if (!currentStates.ContainsKey(this.StateType))
            {
                currentStates.Remove(this.StateType);
                currentStates.Add(this.StateType, this);
            }            
            
        }        

        /// <summary>
        /// 상태머신 진입 가능 여부 리턴
        /// </summary>
        /// <returns> true / false</returns>
        public abstract bool CanEnter();

        /// <summary>
        /// run when enter the state 
        /// </summary>
        public virtual void Enter()
        {
            playerMovement.now_Node = this;
        }
        /// <summary>
        /// run when each frame
        /// </summary>
        public abstract void Update(KeyCode input = KeyCode.None);
        /// <summary>
        /// run when exit the state 
        /// </summary>
        public abstract void Exit();

        public void SetPlyaerMovement(ref PlayerController playerController)
        {
            playerMovement = playerController;
        }

        protected bool ChangeState(PlayerFSMEnum state)
        {
            if (targetStates == null)
            {
                return false;
            }

            foreach (var item in targetStates)
            {
                if (state == item)
                {
                    this.Exit();

                    currentStates[state].Enter();

                }

            }
            return false;



        }


    }



    public class IDLE_Node : State_Node
    {

        protected override void Constructor()
        {
            this.stateType = PlayerFSMEnum.IDLE;
            this.key = KeyCode.None;

            targetStates = new List<PlayerFSMEnum>();
            targetStates.Add(PlayerFSMEnum.MOVE);
            targetStates.Add(PlayerFSMEnum.JUMP);            

            base.Constructor();
        }


        public override bool CanEnter()
        {
            return true;
        }

        public override void Enter()
        {
            base.Enter();
            //애니메이션 추가 할 것
        }

        public override void Exit()
        {
            
        }

        public override void Update(KeyCode input)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            if (horizontalInput > 0.1f ||
                verticalInput > 0.1f)
            {
                if (currentStates[targetStates[0]].CanEnter())
                {
                    currentStates[targetStates[0]].Enter();
                }
            }
            if (Input.GetAxis("Jump") > 0.1)
            {
                
            }


        }


    }

    public class Move_Node : State_Node
    {
        public override bool CanEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void Enter()
        {
            base.Enter();
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(KeyCode input)
        {
            throw new System.NotImplementedException();
        }
    }


    public class FSM_Booter 
    {

        public static State_Node BootFSM(ref PlayerController player)
        {
            State_Node tmp;
            
            tmp = new Move_Node();
            tmp.SetPlyaerMovement(ref player);





            tmp = new IDLE_Node();
            tmp.SetPlyaerMovement(ref player);

            return tmp;
        }

    }




}