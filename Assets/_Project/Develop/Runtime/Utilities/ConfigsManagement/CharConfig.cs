using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagement
{
	[CreateAssetMenu(menuName = "Configs/Gameplay/CharConfig", fileName = "CharConfig")]
	public class CharConfig : ScriptableObject
	{
		[field: SerializeField] public List<char> CharsForGenerate { get; private set; }
	}
}
