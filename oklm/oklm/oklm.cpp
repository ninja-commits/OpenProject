#include "stdafx.h"
#include <vector>
#include <iostream>
#include <windows.h>
#include "ProcMem.h"
#include <iterator>
#include <fstream>
#include <algorithm>
ProcMem Mem; //shortcut 
using namespace std;

const DWORD dwLocalPlayer = 0xD2FB84; //Local Player 
const DWORD dwEntityList = 0x4D43AB4; //Entity list 
const DWORD m_iCrosshairId = 0xB3D4; //0xB2A4
const DWORD m_iTeamNum = 0xF4;
const DWORD m_iHealth = 0x100;

struct Process
{
	DWORD procid;
	HANDLE hproc;
	DWORD clientdll;
};

struct PlayerInfo
{
	DWORD localbase;
	int localteam;
	int CHid;
	DWORD entitybase;
	int entteam;
	int enthealth;
};

class A
{
public:
	A()
	{
		enemy = 0;
		local = 0;
		chid = 0;
	}
	int enemy;
	int local;
	int chid;
};

Process process;
PlayerInfo info;


//void Trigger(int c, int l, DWORD cl)
//{
//	//DWORD EnemyInCH = Mem.Read<DWORD>(cl + dwEntityList + ((c - 1) * EntLoopDist));
//	int EnemyHealth = Mem.Read<int>(EnemyInCH + m_iHealth);
//	int EnemyTeam = Mem.Read<int>(EnemyInCH + m_iTeamNum);
//
//	if (l != EnemyTeam && EnemyHealth > 0)
//	{
//		mouse_event(MOUSEEVENTF_LEFTDOWN, NULL, NULL, NULL, NULL);
//		Sleep(10);
//		mouse_event(MOUSEEVENTF_LEFTUP, NULL, NULL, NULL, NULL);
//	}
//}

void Trigger(A *display)
{
	ReadProcessMemory(Mem.hProcess, (LPCVOID)(process.clientdll + dwLocalPlayer), &info.localbase, sizeof(DWORD), nullptr);
	Sleep(1);
	ReadProcessMemory(Mem.hProcess, (LPCVOID)(info.localbase + m_iTeamNum), &info.localteam, sizeof(int), nullptr);
	ReadProcessMemory(Mem.hProcess, (LPCVOID)(info.localbase + m_iCrosshairId), &info.CHid, sizeof(int), nullptr);
	Sleep(1);
	ReadProcessMemory(Mem.hProcess, (LPCVOID)(process.clientdll + dwEntityList + ((info.CHid - 1) * 16)),
	                  &info.entitybase,
	                  sizeof(DWORD), nullptr);
	ReadProcessMemory(Mem.hProcess, (LPCVOID)(info.entitybase + m_iTeamNum), &info.entteam, sizeof(int), nullptr);
	ReadProcessMemory(Mem.hProcess, (LPCVOID)(info.entitybase + m_iHealth), &info.enthealth, sizeof(int), nullptr);
	Sleep(1);


	if (info.CHid > 0 && info.CHid <= 64 && info.entteam != NULL && info.entteam != info.localteam)
	{
		mouse_event(MOUSEEVENTF_LEFTDOWN, NULL, NULL, NULL, NULL);
		Sleep(10);
		mouse_event(MOUSEEVENTF_LEFTUP, NULL, NULL, NULL, NULL);

		
	}
}

int main()
{
	A *display = new A();
	Mem.Process(_strdup("csgo.exe"));
	std::string name = "client_panorama.dll";
	DWORD ClientDll = Mem.Module(const_cast<LPSTR>(name.c_str()));
	DWORD LocalPlayer = Mem.Read<DWORD>(ClientDll + dwLocalPlayer);

	process.clientdll = ClientDll;
	int loop = 1;


	// print the numbers to stdout
	while (true)
	{
		//Trigger(Mem.Read<int>(LocalPlayer + m_iCrosshairId), Mem.Read<int>(LocalPlayer + m_iTeamNum), ClientDll);
		if (GetAsyncKeyState(VK_F4) & 1)
		{
			if (loop == 1)
			{
				loop = 0;
				cout << "desactive\n";
				cout << '\a';
			}
			else
			{
				loop = 1;
				cout << "active\n";
				cout << '\a';
				cout << '\a';
			}
		}
		if (loop == 1)
		{
			Trigger(display);
			Sleep(1);
		}
	}
	return 0;
}
