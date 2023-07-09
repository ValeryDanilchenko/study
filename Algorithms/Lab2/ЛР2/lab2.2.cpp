#include<iostream>
using namespace std;

int lcs_len(string str1, string str2)
{
	int m, n, left, right;
	string temp;

	m = str1.length();
	n = str2.length();

	if (m == 0 || n == 0)
		return 0;

	if (str1[0] == str2[0])
	{
		str1.erase(0, 1);
		str2.erase(0, 1);
		return lcs_len(str1, str2) + 1;
	}
	else
	{
		temp = str1;
		str1.erase(0, 1);
		left = lcs_len(str1, str2);
		str2.erase(0, 1);
		right = lcs_len(temp, str2);

		if (left > right)
			return left;
		else
			return right;
	}
}


void task_2_2()
{
	string str1 = "abcdef";
	string str2 = "xaycdzxxe";

	cout << "string 1 = " << str1 << "\n" << "string 2 = " << str2 << "\n";
	cout << "LCS lenght = " << lcs_len(str1, str2);
}


int main()
{
	task_2_2();
	return 1;
}