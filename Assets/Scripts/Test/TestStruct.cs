using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// 구조체에 대한 성능 테스트입니다.
/// </summary>
public class TestStruct : MonoBehaviour
{
    private class NormalClass
    {
        public int a, b, c, d, e, f, g;

        public NormalClass(int a = 1)
        {
            this.a = a;
            this.b = 2;
            this.c = 3;
            this.d = 4;
            this.e = 5;
            this.f = 6;
            this.g = 7;
        }

        public void CalcNumber(int index)
        {
            a = index * 444;
            b = a / 257;
            c = b % 11;
            d = c * 999;
            e = d % 144;
            f = e * 777;
            g = f / 141;
        }
    }
    private struct NormalStruct
    {
        public int a, b, c, d, e, f, g;

        public NormalStruct(int a = 1)
        {
            this.a = a;
            this.b = 2;
            this.c = 3;
            this.d = 4;
            this.e = 5;
            this.f = 6;
            this.g = 7;
        }

        public void CalcNumber(int index)
        {
            a = index * 444;
            b = a / 257;
            c = b % 11;
            d = c * 999;
            e = d % 144;
            f = e * 777;
            g = f / 141;
        }
    }

    // 구조체와 클래스 단순 비교
    [ContextMenu("구조체와 클래스 단순 비교")]
    private void StructAndClassTest()
    {
        // 변수 준비
        int length = 1000000;
        Stopwatch watch = new Stopwatch();
        // 구조체 생성 테스트
        watch.Start();
        NormalStruct[] structs = new NormalStruct[length];
        watch.Stop();
        double create_1 = watch.ElapsedMilliseconds * 0.001d;
        watch.Reset();
        // 클래스 생성 테스트
        watch.Start();
        NormalClass[] classes = new NormalClass[length];
        for (int i = 0; i < length; ++i) {
            classes[i] = new NormalClass();
        }
        watch.Stop();
        double create_2 = watch.ElapsedMilliseconds * 0.001d;
        watch.Reset();
        // 구조체 반복 테스트
        watch.Start();
        for (int i = 0; i < length; ++i) {
            structs[i].CalcNumber(i);
        }
        watch.Stop();
        double loop_1 = watch.ElapsedMilliseconds * 0.001d;
        watch.Reset();
        // 클래스 반복 테스트
        watch.Start();
        for (int i = 0; i < length; ++i) {
            classes[i].CalcNumber(i);
        }
        watch.Stop();
        double loop_2 = watch.ElapsedMilliseconds * 0.001d;
        watch.Reset();
        // 결과 출력
        De.Print($"({structs.Length}) ({classes.Length}) ({structs[length / 2].d}) ({classes[length / 2].d})");
        De.Print($"구조체가 클래스보다 생성 속도가 {(create_2 / create_1):F3}배 더 빠릅니다.");
        De.Print($"구조체가 클래스보다 반복 속도가 {(loop_2 / loop_1):F3}배 더 빠릅니다.");
    }
}
