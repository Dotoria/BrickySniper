using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; set; }

        public List<AudioSource> audioSources;

        private void Awake()
        {
            // static
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            PlaySource(5);

            foreach (var obj in FindObjectsOfType<Button>())
            {
                obj.onClick.AddListener(() => PlaySource(2));
                if (obj.gameObject.name == "TouchImage")
                {
                    obj.onClick.RemoveListener(() => PlaySource(2));
                    obj.onClick.AddListener(() => PlaySource(4));
                }
            }
        }

        public void PlaySource(int index)
        {
            audioSources[index].Play();
        }

        public void BackSound(GameObject slider)
        {
            audioSources[5].volume = slider.GetComponent<Slider>().value;
            audioSources[6].volume = slider.GetComponent<Slider>().value;
        }

        public void EffectSound(GameObject slider)
        {
            audioSources[0].volume = slider.GetComponent<Slider>().value;
            audioSources[1].volume = slider.GetComponent<Slider>().value;
            audioSources[2].volume = slider.GetComponent<Slider>().value;
            audioSources[3].volume = slider.GetComponent<Slider>().value;
            audioSources[4].volume = slider.GetComponent<Slider>().value;
        }
    }
}