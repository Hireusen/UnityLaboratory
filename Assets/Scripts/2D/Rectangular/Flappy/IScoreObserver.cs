/// <summary>
/// 누가 바뀌었는가? (Subject), 새 점수도 같이 전달
/// </summary>
public interface IScoreObserver
{
    void OnScoreChanged(ScoreSubject_Classic subject, int newScore);
}
