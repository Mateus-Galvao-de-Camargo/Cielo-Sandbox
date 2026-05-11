function startLoading(buttonId) {
  const button = document.getElementById(buttonId);
  if (button) {
    button.disabled = true;
    const originalText = button.innerHTML;
    button.innerHTML =
      '<span class="spinner-border me-2" role="status" aria-hidden="true"></span><span>Processando...</span>';
    button.dataset.originalHTML = originalText;
  }
}

function stopLoading(buttonId) {
  const button = document.getElementById(buttonId);
  if (button) {
    button.disabled = false;
    button.innerHTML = button.dataset.originalHTML || "Enviar";
    delete button.dataset.originalHTML;
  }
}

document.addEventListener("submit", function (e) {
  const form = e.target;
  if (!(form instanceof HTMLFormElement)) return;
  const button = form.querySelector('button[type="submit"]');
  if (button && button.id) {
    startLoading(button.id);
  }
});

// Máscara monetária pro input de transação
(function () {
  const MAX_CENTS = 999999999999;

  function formatCurrency(cents) {
    cents = Math.max(0, Math.floor(cents));
    const reais = Math.floor(cents / 100);
    const centavos = cents % 100;
    return (
      "R$ " +
      reais.toLocaleString("pt-BR") +
      "," +
      centavos.toString().padStart(2, "0")
    );
  }

  function getCentsFromValue(value) {
    const digits = (value || "").replace(/\D/g, "");
    return digits === "" ? 0 : parseInt(digits, 10);
  }

  function syncHidden(input, cents) {
    const form = input.closest("form");
    if (!form) return;
    const hidden = form.querySelector("[data-currency-value]");
    if (hidden) {
      hidden.value = (cents / 100).toFixed(2);
    }
  }

  function applyValue(input, cents) {
    input.value = formatCurrency(cents);
    syncHidden(input, cents);
    try {
      input.setSelectionRange(input.value.length, input.value.length);
    } catch (_) {}
  }

  function isMaskInput(el) {
    return el && el.matches && el.matches("input[data-currency-mask]");
  }

  document.addEventListener("keydown", function (e) {
    if (!isMaskInput(e.target)) return;
    if (e.ctrlKey || e.metaKey || e.altKey) return;

    const input = e.target;
    let cents = getCentsFromValue(input.value);
    const key = e.key;

    if (key === "Backspace" || key === "Delete") {
      e.preventDefault();
      cents = Math.floor(cents / 10);
      applyValue(input, cents);
      return;
    }

    if (/^[0-9]$/.test(key)) {
      e.preventDefault();
      if (cents < MAX_CENTS) {
        cents = cents * 10 + parseInt(key, 10);
      }
      applyValue(input, cents);
      return;
    }

    const allowed = [
      "Tab",
      "Enter",
      "Escape",
      "ArrowLeft",
      "ArrowRight",
      "ArrowUp",
      "ArrowDown",
      "Home",
      "End",
      "Shift",
      "Control",
      "Alt",
      "Meta",
      "CapsLock",
    ];
    if (allowed.includes(key) || (key.length > 1 && key.startsWith("F"))) {
      return;
    }

    e.preventDefault();
  });

  document.addEventListener("paste", function (e) {
    if (!isMaskInput(e.target)) return;
    e.preventDefault();
    const input = e.target;
    const pasted = (e.clipboardData || window.clipboardData).getData("text");
    const digits = pasted.replace(/\D/g, "");
    let cents = getCentsFromValue(input.value);
    for (const d of digits) {
      if (cents < MAX_CENTS) {
        cents = cents * 10 + parseInt(d, 10);
      }
    }
    applyValue(input, cents);
  });

  document.addEventListener("focusin", function (e) {
    if (!isMaskInput(e.target)) return;
    const input = e.target;
    setTimeout(function () {
      try {
        input.setSelectionRange(input.value.length, input.value.length);
      } catch (_) {}
    }, 0);
  });

  document.addEventListener("mouseup", function (e) {
    if (!isMaskInput(e.target)) return;
    const input = e.target;
    setTimeout(function () {
      try {
        input.setSelectionRange(input.value.length, input.value.length);
      } catch (_) {}
    }, 0);
  });

  document.addEventListener("input", function (e) {
    if (!isMaskInput(e.target)) return;
    const input = e.target;
    const cents = getCentsFromValue(input.value);
    const formatted = formatCurrency(cents);
    if (input.value !== formatted) {
      input.value = formatted;
    }
    syncHidden(input, cents);
  });

  function reformatAll() {
    document.querySelectorAll("input[data-currency-mask]").forEach(function (input) {
      const cents = getCentsFromValue(input.value);
      input.value = formatCurrency(cents);
      syncHidden(input, cents);
    });
  }

  if (typeof Blazor !== "undefined" && Blazor.addEventListener) {
    Blazor.addEventListener("enhancedload", reformatAll);
  }
  document.addEventListener("enhancedload", reformatAll);
  document.addEventListener("DOMContentLoaded", reformatAll);
})();
