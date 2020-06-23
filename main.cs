using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static Dictionary<int, Node> nodes;
    
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        nodes = Enumerable.Range(0, int.Parse(inputs[0])).ToDictionary(i => i, i=> new Node()); // the total number of nodes in the level, including the gateways
        int L = int.Parse(inputs[1]); // the number of links
        int E = int.Parse(inputs[2]); // the number of exit gateways
        /***
          Je crée des list contenant les liens entre les noeuds.
          Du au format des données les liens sont stockés en deux listes.
          Le premier numéro est considéré comme le point de départ et le second comme le point d'arrivé.
        ***/
        foreach(var l in Enumerable.Range(0, L).Select(i => Console.ReadLine().Split()).Select(s => new {From=int.Parse(s[0]), To=int.Parse(s[1])}))
        {
            nodes[l.From].Connections.Add(l.To);
            nodes[l.To].Connections.Add(l.From);
        }
        /***
          J'ajoute un poids à noeud.
          Les poids seront évalué en fonction de la proximité de skynet.
        ***/
        foreach(var g in  Enumerable.Range(0, E).Select(i => int.Parse(Console.ReadLine())))
            nodes[g].Weight = 1;

        // game loop
        while (true)
        {
            int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            
            var to = nodes[SI].Connections.OrderBy(c => nodes[c].Weight).First();
            
            Console.WriteLine(SI +" "+to); // Example: 0 1 are the indices of the nodes you wish to sever the link between
            
            nodes[SI].Connections.Remove(to);
            nodes[to].Connections.Remove(SI);
            
            //@TODO changé le calcul des poids par un calcul fonctionnelle
            foreach(var v in nodes.Values.Where(n => n.Weight > 1))
                v.Weight = int.MaxValue;
            foreach(var v in nodes.Values.Where(n => n.Weight == 1))
                v.Weight = 1;
        }
    }
    
    class Node
    {
        private int _Weight=int.MaxValue;
        public int Weight 
        {
            get { return _Weight; }
            set 
            { 
                _Weight = value;
                if(value != int.MaxValue)
                {
                    foreach(var n in Connections.Select(c => nodes[c]).Where(c => c.Weight > value + 1))
                        n.Weight = value + 1;
                }
            }
        }
        
        public IList<int> Connections = new List<int>();
    }
}
