using System;
using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.PlayerInput
{	
	public class PlayerInputHandler : MonoBehaviour
	{
		public event Action<string> IsTyped;

		[SerializeField] private TMP_InputField _inputField;

		private void Awake()
		{
			_inputField.onEndEdit.AddListener(OnEndEdit);
		}

		private void OnEndEdit(string text)
		{
			IsTyped?.Invoke(text);
		}
	}
}
