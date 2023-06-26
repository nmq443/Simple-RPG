using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    [SerializeField] private int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    int currentLine = 0;
    bool isTyping;
    Dialog dialog;

    public static DialogManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        dialogBox.SetActive(false);
    }

    public IEnumerator ShowDialog(Dialog dialog) {
        yield return new WaitForEndOfFrame();
        OnShowDialog.Invoke();
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public IEnumerator TypeDialog(string line) {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray()) {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

    public void HandleUpdate() {
        if (Input.GetKeyUp(KeyCode.Z) && !isTyping) {
            ++currentLine;
            if (currentLine < dialog.Lines.Count) {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            } else {
                currentLine = 0;
                dialogBox.SetActive(false);
                OnHideDialog.Invoke();
            }
        }
    }
}
