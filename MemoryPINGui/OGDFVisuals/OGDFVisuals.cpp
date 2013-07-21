// OGDFVisuals.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "OGDFVisuals.h"
#include <string>

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);


#include <ogdf/basic/Graph.h>
#include <ogdf\basic\GraphAttributes.h>
#include <ogdf/basic/graph_generators.h>
#include <ogdf/layered/DfsAcyclicSubgraph.h>
#include <ogdf/layered/SugiyamaLayout.h>
#include <ogdf/layered/OptimalRanking.h>
#include <ogdf/layered/MedianHeuristic.h>
#include <ogdf/layered/OptimalHierarchyLayout.h>
#include <ogdf/energybased/FMMMLayout.h>
#include <ogdf/module/CCLayoutPackModule.h>
#include <ogdf/planarity/PlanarizationGridLayout.h>
using namespace ogdf;


//#include <graphviz\gvc.h>
//#include <graphviz\cgraph.h>
//#include <graphviz\graph.h>
#include <stdlib.h>
#include <stdio.h>

char* workaround(char *buf, int n, FILE* fp)
{
  return fgets(buf,n,fp);
}

void agerror(int code, char *str)
{
   printf("my agerror : error code %d : %s\\n", code, str);
}

int APIENTRY _tWinMain(HINSTANCE hInstance,
	HINSTANCE hPrevInstance,
	LPTSTR    lpCmdLine,
	int       nCmdShow)
{
	/*
	GVC_t *gvc;
	Agraph_t *g;
	FILE *fp;
	gvc = gvContext();
	
	fp = fopen("D:\\Documents\\Code\\MemoryPIN\\MemoryPINGui\\OGDFVisuals\\ginput.txt", "r");
	if(fp == 0)
	{
		// flip our shit
		int y = 1;
		int x = 3 / --y;
	}
	char* s = strerror(errno);

	//g = agread(fp); // crashes for some reason...
	g = agopen("D:\\Documents\\Code\\MemoryPIN\\MemoryPINGui\\OGDFVisuals\\ginput2.txt", AGDIGRAPH);

	FILE* output = fopen("D:\\Documents\\Code\\MemoryPIN\\MemoryPINGui\\OGDFVisuals\\out2.txt","w");
	gvLayout(gvc, g, "dot");
	gvRender(gvc, g, "plain", output);
	fclose(output);
	gvRenderFilename(gvc, g, "png", "D:\\Documents\\Code\\MemoryPIN\\MemoryPINGui\\OGDFVisuals\\out.png");
	gvFreeLayout(gvc, g);
	agclose(g);
	gvFreeContext(gvc);
	*/

	
	/*
	randomSimpleGraph(G, 10, 20);
	DfsAcyclicSubgraph DAS;
	DAS.callAndReverse(G);
	G.writeGML("test.gml");
	*/

	Graph G;
	GraphAttributes GA(G, GraphAttributes::nodeGraphics |	
		GraphAttributes::edgeGraphics );
	GA.initAttributes(GraphAttributes::nodeLabel);

	// generate some random nodes
#define NODECOUNT 12
#define EDGECOUNT 1500
	node thenodes[NODECOUNT];
	for(int i = 0; i < NODECOUNT; i++)
	{
		thenodes[i] = G.newNode();
		std::string s;
		std::stringstream out;
		out << i;
		s = out.str();
		GA.labelNode(thenodes[i]) = s.c_str();
	}

	srand(time(0));
	/*
	for(int i = 0; i < EDGECOUNT; i++)
	{
		G.newEdge(thenodes[rand() % NODECOUNT], thenodes[rand() % NODECOUNT]);
	}

	*/

	
	G.newEdge(thenodes[0],thenodes[1]);
	G.newEdge(thenodes[0],thenodes[11]);
	G.newEdge(thenodes[1],thenodes[2]);
	G.newEdge(thenodes[2],thenodes[3]);
	G.newEdge(thenodes[3],thenodes[4]);
	G.newEdge(thenodes[4],thenodes[5]);
	G.newEdge(thenodes[5],thenodes[6]);
	G.newEdge(thenodes[6],thenodes[7]);
	G.newEdge(thenodes[7],thenodes[8]);
	G.newEdge(thenodes[7],thenodes[11]);
	G.newEdge(thenodes[8],thenodes[9]);
	G.newEdge(thenodes[9],thenodes[10]);
	G.newEdge(thenodes[10],thenodes[11]);
	
	node v;
	forall_nodes(v,G)
	{
		GA.width(v) = GA.height(v) = 10.0;
	}
 
	
	PlanarizationGridLayout pgl;
	pgl.call(GA);
	
	/*
	
	FMMMLayout fmmm;
 
	fmmm.useHighLevelOptions(true);
	fmmm.edgeLengthMeasurement(FMMMLayout::EdgeLengthMeasurement::elmBoundingCircle);
	//fmmm.unitEdgeLength(150.0); 
	fmmm.newInitialPlacement(true);
	fmmm.qualityVersusSpeed(FMMMLayout::qvsNiceAndIncredibleSpeed); // change this for various quality
	fmmm.minDistCC(50);
	fmmm.call(GA);
	*/
	
	//GA.writeXML("D:\\Documents\\Code\\MemoryPIN\\MemoryPINGui\\OGDFVisuals\\manual_graph.xml");
	GA.writeGML("D:\\Documents\\Code\\MemoryPIN\\MemoryPINGui\\OGDFVisuals\\manual_graph.gml");
	GA.writeSVG("D:\\Documents\\Code\\MemoryPIN\\MemoryPINGui\\OGDFVisuals\\manual_graph.svg");

	return 0;
	
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_OGDFVISUALS, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_OGDFVISUALS));

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    This function and its usage are only necessary if you want this code
//    to be compatible with Win32 systems prior to the 'RegisterClassEx'
//    function that was added to Windows 95. It is important to call this function
//    so that the application will get 'well formed' small icons associated
//    with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_OGDFVISUALS));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_OGDFVISUALS);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	HWND hWnd;

	hInst = hInstance; // Store instance handle in our global variable

	hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

	if (!hWnd)
	{
		return FALSE;
	}

	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		// TODO: Add any drawing code here...
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
