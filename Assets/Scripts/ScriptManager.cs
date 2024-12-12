using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public List<Sprite> images;

    private bool _isScripting = false;
    private string script = @"...렇게 해서 왕자와 공주님은 오래오래 행복하게 살았답니다.
...뭐야 자는거야? 졸린거 참고 책읽어주고 있는데!!
.... 정말 재미없는 뻔한 결말이야. 나도 낮잠이나 자야겠어.

으음... 살짝 추운데,,,

“ 이 아이를 왕자로 만들어 공주와 결혼해야 여기서 나갈 수 있다. ”
뭐래;;
...?
악 뭐야!! 분명 책보다가 잠들었는데!!! 꿈인가,,
설마 책속으로 들어온거야?
아까 뭐라고? 누구를 왕자로 만들어?

누군데!! 그거 어떻게 하는건데!!!

너는 누구지? 너도 공주님이 결혼할 왕자를 찾고있다는 소식을 들은거야?
으아아ㅏ 뭐야 넌!!
역시 공주님이 결혼할 왕자를 찾는다는 소문 들었군!
우리가 도울게!
그게 무슨.... 
공주님이 결혼하는 날 성대한 파티를 연다고 들었어!
얼마나 맛있는 음식이 많겠어~~ 난 꼭 결혼식을 열거야
무슨 논리람,,,
아... 아무튼... 어떻게 나를 돕겠다는건데??
같이 이 남자를 왕자로 만들면 돼!!
그러니까 뭘 어떻게,,,
우리는 광석을 캐는 숲의 요정들이지!
마쟈마쟈 힘도 짱쎔!

이걸 많이 캐서 돈을 모으는거야! 부자가 되어서 저 성을 사면 이 남자는 왕자가 될 수 있어!! (또는) 광석을 캐서 궁전을 짓고 그 궁전의 주인이 되면 왕자님이 될 수 있어!! (선택지2)
그래 일단... 여기서 빨리 나가야겠어.
그러기 위해서 뭐라도 해보는거야...!
";
    private Coroutine _currentCoroutine;

    public void Scripting()
    {
        if (_isScripting)
        {
            CompleteScript();
            return;
        }

        _isScripting = true;
        _currentCoroutine = StartCoroutine(StartScripting());
    }

    IEnumerator StartScripting()
    {
        text.text = "";

        for (int i = 0; i < script.Length; i++)
        {
            char nextChar = script[i];

            if (nextChar != '\n')
            {
                text.text += nextChar;
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(0.05f);

            if (!_isScripting)
                break;
        }

        CompleteScript();
    }

    private void CompleteScript()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        
        int stopIndex = script.IndexOf('\n');
        if (stopIndex != -1)
        {
            text.text = script.Substring(0, stopIndex);
            script = script.Substring(stopIndex + 1);
        }
        else
        {
            text.text = script;
            SceneLoader.LoadSceneByName("MainScene");
        }

        _isScripting = false;
    }
}