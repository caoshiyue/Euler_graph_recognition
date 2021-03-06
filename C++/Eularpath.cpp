// Dll1.cpp: 定义 DLL 应用程序的导出函数。
//
#include "stdafx.h"
#include <iostream>
#include <string>
#include <algorithm>
#include <list>
using namespace std;
	class Graph
	{
	public:
		Graph() { s = ""; }
		~Graph() { delete[] adj; }
		string s;
		void initadj(int v, int * a);
		void printEulerTour();
	private:
		int V;    //vertex num
		list<int> *adj;    // adjacency lists
		void addEdge(int u, int v)
		{
			adj[v].push_back(u);
			adj[u].push_back(v);
		}
		void rmvEdge(int u, int v);
		void printEulerUtil(int u);
		int DFSCount(int v, bool visited[]);
		bool isValidNextEdge(int u, int v);
	}g;

	void Graph::printEulerTour()
	{
		s = "";
		int u = 0, n = 0;
		for (int i = 0; i < V; i++)
			if (adj[i].size() & 1)
			{
				u = i;
				n++;
			}
		if (n == 2 || n == 0)//odd vertex 
		{
			printEulerUtil(u);
			list<int>::iterator i;
			for (int m = 0; m < V; m++) //search completed but remain edges 
				for (i = adj[m].begin(); i != adj[m].end(); ++i)
					if (*i > 0)
					{
						s = "";
						return;
					}
		}
	}

	void Graph::printEulerUtil(int u)
	{
		list<int>::iterator i;
		for (i = adj[u].begin(); i != adj[u].end(); ++i)
		{
			int v = *i;
			if (v != -1 && isValidNextEdge(u, v))
			{
				char _s[7];
				sprintf_s(_s, "%d-%d ", u, v);
				s += _s;
				rmvEdge(u, v);
				printEulerUtil(v);
			}
		}
	}

	bool Graph::isValidNextEdge(int u, int v)
	{
		// 1) the only edge
		int count = 0;
		list<int>::iterator i;
		for (i = adj[u].begin(); i != adj[u].end(); ++i)
			if (*i != -1)
				count++;
		if (count == 1)
			return true;
		// 2）not a brige
		bool *visited;
		visited = (bool *)malloc(V);
		memset(visited, false, V);
		int count1 = DFSCount(u, visited);
		rmvEdge(u, v);
		memset(visited, false, V);
		int count2 = DFSCount(u, visited);
		addEdge(u, v);
		return (count1 > count2) ? false : true;
	}

	void Graph::initadj(int v, int *a)//adjacent matrix to adjacent list
	{
		V = v;
		adj = new list<int>[V];
		for (int m = 0;m < v;m++)
		{
			for (int n = 0;n <= m;n++)
			{
				int c = (*a + m * v + n);
				for (int i = 0;i < *(a + m * v + n);i++)
					addEdge(m, n);
			}
		}
	}

	void Graph::rmvEdge(int u, int v)//deleted edge is -1
	{
		list<int>::iterator iv = find(adj[u].begin(), adj[u].end(), v);
		*iv = -1;
		list<int>::iterator iu = find(adj[v].begin(), adj[v].end(), u);
		*iu = -1;
	}
	int Graph::DFSCount(int v, bool visited[])
	{
		visited[v] = true;
		int count = 1;
		list<int>::iterator i;
		for (i = adj[v].begin(); i != adj[v].end(); ++i)
			if (*i != -1 && !visited[*i])
				count += DFSCount(*i, visited);
		return count;
	}

//Interface
extern "C" __declspec(dllexport)void initadj(int v, int * a)
{
	g.initadj(v, a);
}
extern "C" __declspec(dllexport)int getpath(string *s)
{
	g.printEulerTour();
	*s = g.s;
	return g.s.length();
}