using UnityEngine;

namespace CodeBase.Services
{
    public class Car : MonoBehaviour
    {
        public void AssignTeam(Team team)
        {
            // Assign team logic
            Debug.Log($"{gameObject.name} assigned to {team} team.");
        }
    }
}