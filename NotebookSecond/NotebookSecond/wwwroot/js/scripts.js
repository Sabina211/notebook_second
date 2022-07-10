document.addEventListener('DOMContentLoaded', () => {
    let count;
    count = document.getElementById("workersCount").value;
    for (var i = 0; i < count; i++) {
        // Модальное окно, которое необходимо открыть
        let modal1 = document.getElementById(`modal-${i}`);
        console.log(modal1);

        // Кнопка "закрыть" внутри модального окна
        let closeButton = modal1.getElementsByClassName('modal__close-button')[0];
        let cancelButton = modal1.getElementsByClassName('modal__cancel-button')[0];

        // Тег body для запрета прокрутки
        let tagBody = document.getElementsByTagName('body');

        /*callBackButton.onclick = function (e) {
          e.preventDefault();
          modal1.classList.add('modal_active');
          tagBody.classList.add('hidden');
        }*/

        closeButton.onclick = function (e) {
            e.preventDefault();
            modal1.classList.remove('modal_active');
            //tagBody.classList.remove('hidden');
        }

        cancelButton.onclick = function (e) {
            e.preventDefault();
            modal1.classList.remove('modal_active');
            //tagBody.classList.remove('hidden');
        }

        modal1.onmousedown = function (e) {
            let target = e.target;
            let modalContent = modal1.getElementsByClassName('modal__content')[0];
            if (e.target.closest('.' + modalContent.className) === null) {
                this.classList.remove('modal_active');
                tagBody.classList.remove('hidden');
            }
        };

        // Вызов модального окна несколькими кнопками на странице
        //let buttonOpenModal1 = document.getElementsByClassName('get-modal_1');
        let buttonOpenModal1 = document.getElementById(`callback-button-${i}`);

        buttonOpenModal1.onclick = function (e) {
            e.preventDefault();
            modal1.classList.add('modal_active');
            //tagBody.classList.add('hidden');
        }

        /*for (let button of buttonOpenModal1) {
            button.onclick = function (e) {
                e.preventDefault();
                modal1.classList.add('modal_active');
                //tagBody.classList.add('hidden');
            }
        }*/
    }
 

});