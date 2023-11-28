using System;
using System.Collections.Generic;

namespace IA2
{
	public class State<T>
    {
		public string Name { get { return _stateName; } }

		public event Action<T> OnEnter = delegate {};
		public event Action OnUpdate = delegate {};
        public event Action OnLateUpdate = delegate { };
        public event Action OnFixedUpdate = delegate { };
		public event Action<T> OnExit = delegate {};

		private string _stateName;
		private Dictionary<T, Transition<T>> transitions;

		public State(string name)
        {
			_stateName = name;
		}

		public State<T> Configure(Dictionary<T, Transition<T>> transitions)
        {
			this.transitions = transitions;
			return this;
		}

		public Transition<T> GetTransition(T input)
        {
			return transitions[input];
	    }

		public bool CheckInput(T input, out State<T> next)
        {
			if(transitions.ContainsKey(input)) {
				var transition = transitions[input];
				transition.OnTransitionExecute(input);
				next = transition.TargetState;
				return true;
			}

			next = this;
			return false;
		}

		public void Enter(T input)
        {
			OnEnter(input);
		}

		public void Update()
        {
			OnUpdate();
		}

        public void LateUpdate()
        {
            OnLateUpdate();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
        }

		public void Exit(T input)
        {
			OnExit(input);
		}
	}
}