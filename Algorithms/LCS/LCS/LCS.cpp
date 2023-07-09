#include <iostream>
#include <vector>
using namespace std;

//int** LCS(string x, string y)
//{
//    int m = x.length() +1;
//    int n = y.length() +1;
//    int** lcs = new int*[m];
//    int** prev = new int* [m];
//    for (int i = 0; i < m + 1; i++)
//    {
//        lcs[i] = new int[n];
//        prev[i] = new int[n];
//    }
//
//    for (int i = 1; i < m; i++)
//        lcs[i][0] = 0;
//    for (int j = 0; j < n; j++)
//        lcs[0][j] = 0;
//
//    for (int i = 0; i < m; i++)
//        for (int j = 0; j < n; j++)
//        {
//            lcs[i][j] = 0;
//            prev[i][j] = 0;
//        }
//
//    for (int i = 1; i < m; i++)
//        for (int j = 1; j < n; j++)
//        {
//            if (x[i] == y[j])
//            {
//                lcs[i][j] = lcs[i - 1][j - 1] + 1;
//            }
//            else
//            {
//                if (lcs[i - 1][j] >= lcs[i][j - 1])
//                    lcs[i][j] = lcs[i - 1][j];
//                //prev[i][j] = pair(i - 1, j)
//                else
//                    lcs[i][j] = lcs[i][j - 1];
//                    //prev[i][j] = pair(i, j - 1)
//            }
//        }
//
//    return lcs;
//
//}


string LCS(const string& x, const string& y)
{
    vector<vector<int> > max_len;
    string result;
    int m = x.size();
    int n = y.size();
    max_len.resize(m + 1);
    for (int i = 0; i <= m; i++)
        max_len[i].resize(n + 1);

    for (int i = m - 1; i >= 0; i--)
    {
        for (int j = n - 1; j >= 0; j--)
        {
            if (x[i] == y[j])
            {
                max_len[i][j] = 1 + max_len[i + 1][j + 1];
            }
            else
            {
                max_len[i][j] = max(max_len[i + 1][j], max_len[i][j + 1]);
            }
        }
    }

    //for (int i = 1; i < m; i++)
    //    max_len[i][0] = 0;
    //for (int j = 0; j < n; j++)
    //    max_len[0][j] = 0;

    //for (int i = 1; i < m; i++)
    //{
    //    for (int j = 1; j < n; j++)
    //    {
    //        if (x[i] == y[j])
    //        {
    //            max_len[i][j] = max_len[i - 1][j - 1] + 1;
    //        }
    //        else
    //        {
    //            max_len[i][j] = max(max_len[i - 1][j], max_len[i][j - 1]);
    //        }
    //    }
    //}
    for (int i = 0, j = 0; max_len[i][j] != 0 && i < m && j < n; )
    {
        if (x[i] == y[j])
        {
            result.push_back(x[i]);
            i++;j++;
        }
        else
        {
            if (max_len[i][j] == max_len[i + 1][j])
                i++;
            else
                j++;
        }
    }
    return result;
}

void printLCS(int i, int j, int ** lcs, string x)
{
    if (i == 0 or j == 0) // пришли к началу LCS
        return;
    if (lcs[i][j] == lcs[i - 1][j - 1] - 1)
    {
        printLCS(i - 1, j - 1, lcs, x);
        cout << x[i];
    }
    else
    {
        if (lcs[i][j] == lcs[i - 1][j])
            printLCS(i - 1, j, lcs, x);
        else
            printLCS(i, j - 1, lcs, x);
    }
}

int main()
{    
    setlocale(LC_ALL, "RU");
    string s1 = "abcdef";
    string s2 = "xaycdzxxe";

    cout << "Наибольшая общая подпоследовательность строк " << s1 << " и " << s2 << ":\n";
    cout << LCS(s1, s2);
}

