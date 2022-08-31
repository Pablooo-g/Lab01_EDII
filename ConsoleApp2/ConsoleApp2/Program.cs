using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            leerCSV();
        }
        static void leerCSV()
        {
            var reader = new StreamReader(File.OpenRead(@"C:\Users\pablo\Documentos\Estructuras de Datos II\input.csv"));
            List<String> lista = new List<string>();
            while (!reader.EndOfStream)
            {
                var linea = reader.ReadLine();
                var valores = linea.Split(',');
                for (int i = 0; i < valores.Length; i++)
                {
                    if ((i % 3) == 0)
                    {
                        Console.Write(valores[i] + "-" + valores[i + 1] + "-" + valores[i + 2]);
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                }
            }
        }
    }
    public class AVLtree<T>
    {
        private class Nodo
        {
            public T value { get; set; }
            public int height { get; set; }
            public Nodo left { get; set; }
            public Nodo right { get; set; }
            public Nodo(T _value)
            {
                value = _value;
                height = 1;
            }
        }
        private Nodo root;
        public delegate bool compareFuncion(T value, string boolOperator, T value2);
        public List<T> getAll()
        {
            List<T> newList = new List<T>();
            getInOrder(root, newList);
            return newList;
        }
        private void getInOrder(Nodo node, List<T> allData)
        {
            if (node != null)
            {
                getInOrder(node.left, allData);
                allData.Add(node.value);
                getInOrder(node.right, allData);
            }
        }
        public int insert(T value, compareFuncion compare)
        {
            int totalRotations = 0;
            root = insertNode(root, value, compare, ref totalRotations);

            return totalRotations;
        }
        private int max(int a, int b)
        {
            return (a > b) ? a : b;
        }
        private int height(Nodo node)
        {
            if (node == null)
            {
                return 0;
            }
            return node.height;
        }
        private int getBalanceFactor(Nodo node)
        {
            if (node == null)
            {
                return 0;
            }
            return height(node.left) - height(node.right);
        }
        private Nodo rotateRight(Nodo y)
        {
            Nodo x = y.left;
            Nodo T2 = x.right;
            x.right = y;
            y.left = T2;
            y.height = max(height(y.left), height(y.right)) + 1;
            x.height = max(height(x.left), height(x.right)) + 1;

            return x;
        }
        private Nodo rotateLeft(Nodo x)
        {
            Nodo y = x.right;
            Nodo T2 = y.left;
            y.left = x;
            x.right = T2;
            x.height = max(height(x.left), height(x.right)) + 1;
            y.height = max(height(y.left), height(y.right)) + 1;

            return y;
        }
        private Nodo insertNode(Nodo node, T value, compareFuncion compare, ref int totalRotations)
        {
            if (node == null)
            {
                return new Nodo(value);
            }

            if (compare(value, "<", node.value))
            {
                node.left = insertNode(node.left, value, compare, ref totalRotations);
            }
            else if (compare(value, ">", node.value))
            {
                node.right = insertNode(node.right, value, compare, ref totalRotations);
            }
            else
            {
                return node;
            }

            node.height = max(height(node.left), height(node.right)) + 1;
            int balanceFactor = getBalanceFactor(node);
            if (balanceFactor > 1)
            {
                if (compare(value, "<", node.left.value))
                {
                    totalRotations++;
                    return rotateRight(node);
                }
                else if (compare(value, ">", node.left.value))
                {
                    totalRotations += 2;
                    node.left = rotateLeft(node.left);
                    return rotateRight(node);
                }
            }
            if (balanceFactor < -1)
            {
                if (compare(value, ">", node.right.value))
                {
                    totalRotations++;
                    return rotateLeft(node);
                }
                else if (compare(value, "<", node.right.value))
                {
                    totalRotations += 2;
                    node.right = rotateRight(node.right);
                    return rotateLeft(node);
                }
            }

            return node;
        }

        private int depth(Nodo root)
        {
            if (root == null)
            {
                return 0;
            }

            if (root.left == null && root.right == null)
            {
                return 1;
            }

            return 1 + Math.Max(depth(root.left), depth(root.right));
        }

        public int getDepth()
        {
            return depth(root);
        }

        private T Busqueda(T value, compareFuncion compare, Nodo raiz, ref int comparaciones)
        {
            comparaciones++;
            if (raiz == null)
            {
                return default(T);
            }

            if (compare(value, "==", raiz.value))
            {
                return raiz.value;
            }

            if (compare(value, "<", raiz.value))
            {
                return Busqueda(value, compare, raiz.left, ref comparaciones);
            }

            if (compare(value, ">", raiz.value))
            {
                return Busqueda(value, compare, raiz.right, ref comparaciones);
            }

            return default(T);
        }

        public T Search(T value, compareFuncion compare, ref int comparaciones)
        {
            return Busqueda(value, compare, root, ref comparaciones);
        }

        //Sort
        public void Sort(compareFuncion compare)
        {
            List<T> datosarbol = getAll();
            root = null;
            foreach (T value in datosarbol)
            {
                insert(value, compare);
            }
        }
    }
}
