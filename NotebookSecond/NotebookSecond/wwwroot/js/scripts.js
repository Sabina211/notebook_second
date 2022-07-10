document.addEventListener('DOMContentLoaded', () => {
    let count;
    count = document.getElementById("workersCount").value;
    for (var i = 0; i < count; i++) {
        // Модальное окно, которое необходимо открыть
        let modal1 = document.getElementById(`modal-${i}`);

        //модалка редактирования
        let modalEdit = document.getElementById(`modal-edit-${i}`);

        //модалка просмотра
        let modalView = document.getElementById(`modal-view-${i}`);

        //список с модальными окнами
        let modals = new Set();
        modals.add(document.getElementById(`modal-${i}`));
        modals.add(document.getElementById(`modal-edit-${i}`));
        modals.add(document.getElementById(`modal-view-${i}`));

        //список с отменяющими кнопками
        let closeButtons = new Set();
        for (let itemModal of modals) {
            closeButtons.add(itemModal.getElementsByClassName('modal__close-button')[0]);
            closeButtons.add(itemModal.getElementsByClassName('modal__cancel-button')[0]);
        }

        //нажатие на отменяюшие кнопки
        for (let item of closeButtons) {
            item.onclick = function (e) {
                for (let itemModal of modals) {
                e.preventDefault();
                itemModal.classList.remove('modal_active');
                }
            }
        }


        // Тег body для запрета прокрутки
        let tagBody = document.getElementsByTagName('body');

        modal1.onmousedown = function (e) {
            let target = e.target;
            let modalContent = modal1.getElementsByClassName('modal__content')[0];
            if (e.target.closest('.' + modalContent.className) === null) {
                this.classList.remove('modal_active');
                tagBody.classList.remove('hidden');
            }
        };

        modalEdit.onmousedown = function (e) {
            let target = e.target;
            let modalContent = modalEdit.getElementsByClassName('modal__content')[0];
            if (e.target.closest('.' + modalContent.className) === null) {
                this.classList.remove('modal_active');
                tagBody.classList.remove('hidden');
            }
        };

        modalView.onmousedown = function (e) {
            let target = e.target;
            let modalContent = modalView.getElementsByClassName('modal__content')[0];
            if (e.target.closest('.' + modalContent.className) === null) {
                this.classList.remove('modal_active');
                tagBody.classList.remove('hidden');
            }
        };

        // Вызов модального окна несколькими кнопками на странице
        let buttonOpenModal1 = document.getElementById(`callback-button-${i}`);

        buttonOpenModal1.onclick = function (e) {
            e.preventDefault();
            modal1.classList.add('modal_active');
            //tagBody.classList.add('hidden');
        }

        //вызов модалки редактирования
        let buttonOpenModalEdit = document.getElementById(`callback-button-edit-${i}`);

        buttonOpenModalEdit.onclick = function (e) {
            e.preventDefault();
            modalEdit.classList.add('modal_active');
            //tagBody.classList.add('hidden');
        }

        // вызов модалки просмотра
        let buttonOpenModalView = document.getElementById(`callback-button-view-${i}`);

        buttonOpenModalView.onclick = function (e) {
            e.preventDefault();
            modalView.classList.add('modal_active');
            //tagBody.classList.add('hidden');
        }
    }
 

});