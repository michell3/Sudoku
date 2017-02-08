using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefBoard{

	private static int[,] answer1 = {  { 8, 7, 2, 9, 5, 6, 1, 4, 3 },
								{ 1, 3, 5, 7, 8, 4, 9, 2, 6 },
								{ 9, 6, 4, 2, 1, 3, 7, 5, 8 },
								{ 7, 4, 8, 3, 6, 2, 5, 9, 1 },
								{ 2, 5, 6, 1, 4, 9, 3, 8, 7 },
								{ 3, 1, 9, 8, 7, 5, 2, 6, 4 },
								{ 5, 8, 7, 4, 2, 1, 6, 3, 9 },
								{ 6, 9, 1, 5, 3, 8, 4, 7, 2 },
								{ 4, 2, 3, 6, 9, 7, 8, 1, 5 } };

	private static int[,] show1 =  {   { 0, 0, 1, 0, 1, 0, 1, 1, 0 },
								{ 0, 0, 0, 1, 1, 0, 1, 0, 1 },
								{ 0, 0, 0, 0, 0, 1, 0, 0, 1 },
								{ 1, 0, 1, 0, 0, 0, 0, 0, 1 },
								{ 0, 0, 1, 0, 0, 0, 1, 0, 0 },
								{ 1, 0, 0, 0, 0, 0, 1, 0, 1 },
								{ 1, 0, 0, 1, 0, 0, 0, 0, 0 },
								{ 1, 0, 1, 0, 1, 1, 0, 0, 0 },
								{ 0, 1, 1, 0, 1, 0, 1, 0, 0 } };
	static Dictionary<int, int[,]> answers = 
		new Dictionary<int, int[,]>(){
		{ 1, answer1} 
	};

	static Dictionary<int, int[,]> shows = 
		new Dictionary<int, int[,]>(){
		{ 1, show1} 
	};


	public static int[,] getAnswerBoard (int index){
		return answers [index];
	}
	public static int[,] getShowBoard (int index){
		return shows [index];
	}
}
