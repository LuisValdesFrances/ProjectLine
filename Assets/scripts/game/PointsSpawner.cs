using UnityEngine;
using System.Collections;

public class PointsSpawner : MonoBehaviour 
{
    [SerializeField]
    private float distance = 3;
    [SerializeField]
    private float separationNumbers = 0.85f;

    public const int MAX_DIGITS = 5;

    public float maxSize = 1.4f;
    public float minSize = 0.8f;

    

    private float durToBig = 0.15f;
    private float durToUp = 1f;
    
    private float count;

    private int quantity;


    private Vector2 position;
    private float size;

    public enum states { toBig, toSmall, toUp };
    private states state;

    private int numberDigits;
    private SpriteRenderer[] digits;
    public Sprite[] numbers;

    public void Spawn(Vector2 position, int quantity)
    {

        this.digits = GetComponentsInChildren<SpriteRenderer>();
        this.position = position;
        this.quantity = quantity;
        numberDigits = ("" + quantity).Length;



        switch (numberDigits)
        {
            case 1:
                digits[0].transform.localPosition = new Vector2(-separationNumbers / 2f - separationNumbers, 0);
                digits[1].enabled = false;
                digits[2].enabled = false;
                digits[3].enabled = false;
                digits[4].enabled = false;
                break;
            case 2:
                digits[0].transform.localPosition = new Vector2(-separationNumbers / 2f, 0);
                digits[1].transform.localPosition = new Vector2(+separationNumbers / 2f, 0);
                digits[2].enabled = false;
                digits[3].enabled = false;
                digits[4].enabled = false;
                break;
            case 3:
                digits[0].transform.localPosition = new Vector2(-separationNumbers, 0);
                digits[1].transform.localPosition = new Vector2(0, 0);
                digits[2].transform.localPosition = new Vector2(+separationNumbers, 0);
                digits[3].enabled = false;
                digits[4].enabled = false;
                break;
            case 4:
                digits[0].transform.localPosition = new Vector2(-separationNumbers / 2f - separationNumbers, 0);
                digits[1].transform.localPosition = new Vector2(-separationNumbers / 2f, 0);
                digits[2].transform.localPosition = new Vector2(+separationNumbers / 2f, 0);
                digits[3].transform.localPosition = new Vector2(+separationNumbers / 2f + separationNumbers, 0);
                digits[4].enabled = false;
                break;
            case 5:
                digits[0].transform.localPosition = new Vector2(-separationNumbers * 2f, 0);
                digits[1].transform.localPosition = new Vector2(-separationNumbers, 0);
                digits[2].transform.localPosition = new Vector2(0, 0);
                digits[3].transform.localPosition = new Vector2(+separationNumbers, 0);
                digits[4].transform.localPosition = new Vector2(-separationNumbers * 2f, 0);
                break;
        }

        //Put digits:
        string p = "" + quantity;

        for (int i = 0; i < numberDigits; i++)
        {

            int par = System.Int32.Parse(p[i].ToString());
            digits[i].sprite = numbers[par];
        }

        transform.position = this.position;
        transform.localScale = new Vector2(0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case states.toBig:
                count += Time.deltaTime;
                if (count > durToBig)
                {
                    count = 0;
                    state = states.toSmall;
                    size = GetObjetiveSize();

                }
                else
                {
                    //float dif = maxSize - minSize;
                    size = (count * GetObjetiveSize()) / durToBig;
                    transform.localScale = new Vector2(size, size);
                }
                break;
            case states.toSmall:
                //En este estado, la escala se reduce a una 3º parte en la mitad de tiempo
                float modScale = 3f;
                float modTime = 2f;


                float decreaseSize = GetObjetiveSize() / modScale;
                float decreaseTime = durToBig / modTime;


                count += Time.deltaTime;
                float modSize = (count * decreaseSize) / decreaseTime;
                if (count < decreaseTime)
                {
                    transform.localScale = new Vector2(size - modSize, size - modSize);
                }
                else
                {
                    size = transform.localScale.x;
                    count = 0;
                    state = states.toUp;

                }
                break;
            case states.toUp:
                count += Time.deltaTime;
                if (count < durToUp)
                {
                    //float modY = (Time.deltaTime * distance) / durToUp;
                    transform.position = (new Vector2(transform.position.x, this.position.y + ((count * distance) / durToUp)));
                    transform.localScale = new Vector2(size - ((count * size) / durToUp), size - ((count * size) / durToUp));
                    for (int i = 0; i < numberDigits; i++)
                    {
                        float alpha = 1f - (count / durToUp);
                        digits[i].color = new Color(digits[i].color.r, digits[i].color.g, digits[i].color.b, alpha);
                    }
                    break;
                }
                else
                {
                    Destroy(gameObject);
                }

                break;
        }
    }

    private float GetObjetiveSize()
    {
        float dif = maxSize - minSize;
        return minSize + ((quantity * dif) / 99999f);//99999 es el maximo que se puede conseguir con 5 cifras
    }
}
