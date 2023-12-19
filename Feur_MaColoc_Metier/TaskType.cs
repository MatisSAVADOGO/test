using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Metier
{
    public class TaskType
    {
        private int id;
        private string name;
        private int duration;
        private string repetition;

        /// <summary>
        /// Id de la tâche créé
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// nom de la tâche
        /// </summary>
        public string Name { get => name; set => name = value;}
        /// <summary>
        /// durée de la tâche
        /// </summary>
        public int Duration { get => duration; set => duration = value; }
        public string Repetition { get => repetition; set => repetition = value; }



        /// <summary>
        /// constructeur de la classe, c'est une tâche
        /// </summary>
        /// <param name="name">nom de la tâche</param>
        /// <param name="duration">durée que doit prendre la tâche</param>
        public TaskType(int id, string name, int duration, string repetition) 
        {
            this.id = id;   
            this.name = name;
            this.duration = duration;
            this.repetition = repetition;
        }
        /// <summary>
        /// autre constructeur mais avec en paramètre uniquement non et durée
        /// </summary>
        /// <param name="name"></param>
        /// <param name="duration"></param>
        public TaskType(string name, int duration, string repetition)
        {
            this.name = name;
            this.duration = duration;
            this.repetition = repetition;
        }
        public TaskType()
        {
            
        }
        ~TaskType() { }  
    }
}