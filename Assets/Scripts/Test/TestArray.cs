using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// 반복에 대해 단순 테스트해봅니다.
/// </summary>
public class TestArray : MonoBehaviour
{
    // 단순히 차원의 유무임에도 1차원 배열이 약 2배 빠름
    [ContextMenu("배열 평탄화 테스트 (1)")]
    private void FlatteningTest()
    {
        int width = 10000;
        int height = 10000;
        int length = width * height;
        int[] gridOne = new int[length];
        int[,] gridTwo = new int[width, height];

        Stopwatch watch = new Stopwatch();
        watch.Start();
        long sum_1 = 0L;
        // 1차원 배열 순환
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                int index = y * width + x;
                gridOne[index] = 999;
                sum_1 += (long)gridOne[index];
            }
        }
        watch.Stop();
        double result_1 = watch.ElapsedMilliseconds * 0.001d;
        De.Print($"1차원 배열({gridOne.Length}) : {result_1}초 (sum = {sum_1})");
        watch.Reset();
        watch.Start();
        long sum_2 = 0L;
        // 2차원 배열 순환
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                gridTwo[y, x] = 999;
                sum_2 += (long)gridTwo[y, x];
            }
        }
        watch.Stop();
        double result_2 = watch.ElapsedMilliseconds * 0.001d;
        De.Print($"2차원 배열({gridTwo.GetLength(1)} * {gridTwo.GetLength(1)}) : {result_2}초 (sum = {sum_2})");
        De.Print($"1차원 배열이 {(result_2 / result_1):F3}배 더 빠릅니다.");
    }

    // 내부 연산을 무겁게 하니 1차원 배열이 약 2배가 아닌 1.7배 정도 빨라짐
    [ContextMenu("배열 평탄화 테스트 (2)")]
    private void FlatteningTest_HardCalc()
    {
        int width = 10000;
        int height = 10000;
        int length = width * height;
        int[] gridOne = new int[length];
        int[,] gridTwo = new int[width, height];

        Stopwatch watch = new Stopwatch();
        watch.Start();
        long sum_1 = 0L;
        // 1차원 배열 순환
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                int index = y * width + x;
                gridOne[index] = x / 999 * y + x + y;
                sum_1 += (long)gridOne[index];
                sum_1 -= (long)(gridOne[index] * 0.01);
                sum_1 += (long)(gridOne[index] * 10.01);
                sum_1 -= (long)(gridOne[index] * 5.1114);
            }
        }
        watch.Stop();
        double result_1 = watch.ElapsedMilliseconds * 0.001d;
        De.Print($"1차원 배열({gridOne.Length}) : {result_1}초 (sum = {sum_1})");
        watch.Reset();
        watch.Start();
        long sum_2 = 0L;
        // 2차원 배열 순환
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                gridTwo[y, x] = x / 999 * y + x + y;
                sum_2 += (long)gridTwo[y, x];
                sum_2 -= (long)(gridTwo[y, x] * 0.01);
                sum_2 += (long)(gridTwo[y, x] * 10.01);
                sum_2 -= (long)(gridTwo[y, x] * 5.1114);
            }
        }
        watch.Stop();
        double result_2 = watch.ElapsedMilliseconds * 0.001d;
        De.Print($"2차원 배열({gridTwo.GetLength(1)} * {gridTwo.GetLength(1)}) : {result_2}초 (sum = {sum_2})");
        De.Print($"1차원 배열이 {(result_2 / result_1):F3}배 더 빠릅니다.");
    }

    // 2차원 배열의 반복 순서를 바꾸어 캐시 효율을 박살냄. 1차원 배열이 약 5.5배 더 빠름
    [ContextMenu("배열 평탄화 테스트 (3)")]
    private void FlatteningTest_CashCalc()
    {
        int width = 10000;
        int height = 10000;
        int length = width * height;
        int[] gridOne = new int[length];
        int[,] gridTwo = new int[width, height];

        Stopwatch watch = new Stopwatch();
        watch.Start();
        long sum_1 = 0L;
        // 1차원 배열 순환
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                int index = y * width + x;
                gridOne[index] = 999;
                sum_1 += (long)gridOne[index];
            }
        }
        watch.Stop();
        double result_1 = watch.ElapsedMilliseconds * 0.001d;
        De.Print($"1차원 배열({gridOne.Length}) : {result_1}초 (sum = {sum_1})");
        watch.Reset();
        watch.Start();
        long sum_2 = 0L;
        // 2차원 배열 순환
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                gridTwo[y, x] = 999;
                sum_2 += (long)gridTwo[y, x];
            }
        }
        watch.Stop();
        double result_2 = watch.ElapsedMilliseconds * 0.001d;
        De.Print($"2차원 배열({gridTwo.GetLength(1)} * {gridTwo.GetLength(1)}) : {result_2}초 (sum = {sum_2})");
        De.Print($"1차원 배열이 {(result_2 / result_1):F3}배 더 빠릅니다.");
    }

    // 2차원 배열의 캐시 효율 박살 * 무거운 연산 => 1차원 배열이 2배 빠름
    [ContextMenu("배열 평탄화 테스트 (4)")]
    private void FlatteningTest_CashHardCalc()
    {
        int width = 10000;
        int height = 10000;
        int length = width * height;
        int[] gridOne = new int[length];
        int[,] gridTwo = new int[width, height];

        Stopwatch watch = new Stopwatch();
        watch.Start();
        long sum_1 = 0L;
        // 1차원 배열 순환
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                int index = y * width + x;
                gridOne[index] = x / 999 * y + x + y;
                sum_1 += (long)gridOne[index];
                sum_1 -= (long)(gridOne[index] * 0.01);
                sum_1 += (long)(gridOne[index] * 10.01);
                sum_1 -= (long)(gridOne[index] * 5.1114);
            }
        }
        watch.Stop();
        double result_1 = watch.ElapsedMilliseconds * 0.001d;
        De.Print($"1차원 배열({gridOne.Length}) : {result_1}초 (sum = {sum_1})");
        watch.Reset();
        watch.Start();
        long sum_2 = 0L;
        // 2차원 배열 순환
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                gridTwo[y, x] = x / 999 * y + x + y;
                sum_2 += (long)gridTwo[y, x];
                sum_2 -= (long)(gridTwo[y, x] * 0.01);
                sum_2 += (long)(gridTwo[y, x] * 10.01);
                sum_2 -= (long)(gridTwo[y, x] * 5.1114);
            }
        }
        watch.Stop();
        double result_2 = watch.ElapsedMilliseconds * 0.001d;
        De.Print($"2차원 배열({gridTwo.GetLength(1)} * {gridTwo.GetLength(1)}) : {result_2}초 (sum = {sum_2})");
        De.Print($"1차원 배열이 {(result_2 / result_1):F3}배 더 빠릅니다.");
    }
}
