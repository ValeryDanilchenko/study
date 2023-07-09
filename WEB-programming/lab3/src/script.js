import Swiper from 'swiper/bundle';
import 'swiper/css/bundle';
// import 'swiper/css/navigation';
// import 'swiper/css/pagination';

document.addEventListener('DOMContentLoaded', function () {
    var swiper = new Swiper('.swiper-container', {
        
        loop: true,
        
        effect: 'fade',
        slidesPerView: 1,
        spaceBetween: 30,
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
    });
});



const depositType = document.getElementById('deposit-type');
const depositTerm = document.getElementById('deposit-term');
const depositAmount = document.getElementById('deposit-amount');
const depositForm = document.getElementById('deposit-form');
const result = document.getElementById('result');

const depositOptions = {
    replenishable: {
        '6 месяцев': 0.2,
        '1 год': 0.22,
        '1,5 года': 0.15,
        '2 года': 0.1
    },
    term: {
        '3 месяца': 0.2,
        '6 месяцев': 0.22,
        '9 месяцев': 0.23,
        '1 год': 0.24,
        '1,5 года': 0.18,
        '2 года': 0.15
    },
    months: {
        '3 месяца': 3,
        '6 месяцев': 6,
        '9 месяцев': 9,
        '1 год': 12,
        '1,5 года': 18,
        '2 года': 24
    }
};

function updateDepositTerms() {
    const type = depositType.value;
    const terms = depositOptions[type];
   
    depositTerm.innerHTML = '';
    for (const term in terms) {
        const option = document.createElement('option');
        option.value = term;
        option.textContent = term;
        depositTerm.appendChild(option);
    }
}

depositType.addEventListener('change', updateDepositTerms);

depositForm.addEventListener('submit', (event) => {
    event.preventDefault();

    const type = depositType.value;
    const term = depositTerm.value;
    const amount = parseFloat(depositAmount.value);
    const rate = depositOptions[type][term];

    const termInMonths = depositOptions.months[term];
    const finalAmount = amount * (1 + rate * termInMonths / 12);

    result.innerHTML = `Вид вклада: ${type === 'replenishable' ? 'Пополняемый' : 'Срочный'}<br>
                        Срок вклада: ${term}<br>
                        Сумма вклада: ${amount.toFixed(2)}<br>
                        Сумма в конце срока: ${finalAmount.toFixed(2)}`;
});

updateDepositTerms();