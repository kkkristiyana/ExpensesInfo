function performCalculation() {
    const num1 = parseFloat(document.getElementById('num1').value); const num2 = parseFloat(document.getElementById('num2').value); const operation = document.getElementById('operation').value;

    if (isNaN(num1) || isNaN(num2)) {
        showResult('Please enter valid numbers.'); return;
    }

    let result; switch (operation) {
        case 'add': result = num1 + num2; break;
        case 'subtract': result = num1 - num2; break;
        case 'multiply': result = num1 * num2; break;
        case 'divide': result = num2 === 0 ? 'Cannot divide by zero' : (num1 / num2); break; 
        case 'power': result = Math.pow(num1,num2); break;
        default: result = 'Invalid operation';
    }

    showResult('Result: ' + result);
}

function clearCalc() {
    document.getElementById('num1').value = ''; document.getElementById('num2').value = ''; document.getElementById('operation').value = 'add'; document.getElementById('result').style.display = 'none';
}

function showResult(text) {
    const box = document.getElementById('result'); box.innerText = text;
    box.style.display = 'block';
}
