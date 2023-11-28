using System;

namespace IA2
{
	public class Transition<T>
    {
		public event Action<T> OnTransition = delegate { };
		public T Input { get { return input; } }
		public State<T> TargetState { get { return targetState;  } }

		T input;
		State<T> targetState;


        public void OnTransitionExecute(T input)
        {
			OnTransition(input);
		}

		public Transition(T input, State<T> targetState)
        {
			this.input = input;
			this.targetState = targetState;
		}
	}
}