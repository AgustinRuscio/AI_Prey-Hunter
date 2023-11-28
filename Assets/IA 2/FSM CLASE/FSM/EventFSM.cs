using System;

namespace IA2
{
	public class EventFSM<T>
    {
		public State<T> Current { get { return current; } }
		private State<T> current;

		public EventFSM(State<T> initial)
        {
			current = initial;
			current.Enter(default(T));
		}

		public void SendInput(T input)
        {
			State<T> newState;

			if (current.CheckInput(input, out newState))
            {
				current.Exit(input);
				current = newState;
				current.Enter(input);
			}
		}


		public void Update()
        {
			current.Update();
		}

        public void LateUpdate()
        {
            current.LateUpdate();
        }

        public void FixedUpdate()
        {
            current.FixedUpdate();
        }
	}
}