using UnityEngine;
using System.Collections;

public class RotMesh : MonoBehaviour
{
    private bool run;
    public bool touched;
    public ParticleSystem dust1, dust2, dust3, dust4;
    private DisableInput disableInput;
    public GameObject backwards, right;

    private void Awake()
    {
        dust1.Stop(); dust2.Stop(); dust3.Stop(); dust4.Stop();
        disableInput = FindObjectOfType<DisableInput>();
    }


    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if(!disableInput.touchedCollision)
            {
                //Backwards
                dust1.Play(); dust2.Play(); dust3.Play(); dust4.Play();
                if (this.transform.eulerAngles.y == 270)
                {
                    right.SetActive(true); backwards.SetActive(false);
                    StartCoroutine(Rotate(-Vector3.up, 90, 0.1f));
                    StartCoroutine(stopParticles());
                    Debug.Log("Goes Backwards");
                }
                //Right
                else
                {
                    backwards.SetActive(true); right.SetActive(false);
                    StartCoroutine(Rotate(Vector3.up, 90, 0.1f));
                    StartCoroutine(stopParticles());
                    Debug.Log("Goes Right");
                }
            }
        }
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            {
                transform.rotation = Quaternion.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.rotation = to;
        }
    }

    IEnumerator stopParticles()
    {
        
        yield return new WaitForSeconds(.5f);
        dust1.Stop(); dust2.Stop(); dust3.Stop(); dust4.Stop();
    }
}