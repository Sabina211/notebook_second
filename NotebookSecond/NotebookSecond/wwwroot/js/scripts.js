document.addEventListener('DOMContentLoaded', () => {
    let workersCount;
    let workers = document.getElementById("workersCount");
    if (workers!=null) {
        workersCount = document.getElementById("workersCount").value;
    }
    
    for (var i = 0; i < workersCount; i++) {
        // Модальное окно, которое необходимо открыть
        let modal1 = document.getElementById(`modal-${i}`);

        //модалка редактирования сотрудника
        let modalEdit = document.getElementById(`modal-edit-${i}`);

        //модалка просмотра сотрудника
        let modalView = document.getElementById(`modal-view-${i}`);

        //список с модальными окнами
        let modals = new Set();
        modals.add(document.getElementById(`modal-${i}`));
        modals.add(document.getElementById(`modal-edit-${i}`));
        modals.add(document.getElementById(`modal-view-${i}`));
        //modals.add(document.getElementById(`modal-user-edit-${i}`));

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
        if (buttonOpenModal1!=null) {
            buttonOpenModal1.onclick = function (e) {
                e.preventDefault();
                modal1.classList.add('modal_active');
                //tagBody.classList.add('hidden');
            }
        }


        //вызов модалки редактирования сотрудника
        let buttonOpenModalEdit = document.getElementById(`callback-button-edit-${i}`);
        if (buttonOpenModalEdit!=null) {
            buttonOpenModalEdit.onclick = function (e) {
                e.preventDefault();
                modalEdit.classList.add('modal_active');
                //tagBody.classList.add('hidden');
            }
        }
  

        // вызов модалки просмотра
        let buttonOpenModalView = document.getElementById(`callback-button-view-${i}`);
        if (buttonOpenModalView!=null) {
            buttonOpenModalView.onclick = function (e) {
                e.preventDefault();
                modalView.classList.add('modal_active');
                //tagBody.classList.add('hidden');
            }
        }
    }

    //страница c пользователями системы
    let usersCount;
    let users = document.getElementById("usersCount");
    if (users!=null) {
        usersCount = document.getElementById("usersCount").value;
    }

    for (var i = 0; i < usersCount; i++) {
        //модалка редактирования пользователя
        let modalUserEdit = document.getElementById(`modal-user-edit-${i}`);

        //модалка удалением пользователя
        let modalUserDelete = document.getElementById(`modal-user-delete-${i}`);

        //список с модальными окнами
        let modals = new Set();
        modals.add(document.getElementById(`modal-user-edit-${i}`));
        modals.add(document.getElementById(`modal-user-delete-${i}`));

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

        modalUserEdit.onmousedown = function (e) {
            let target = e.target;
            let modalContent = modalUserEdit.getElementsByClassName('modal__content')[0];
            if (e.target.closest('.' + modalContent.className) === null) {
                this.classList.remove('modal_active');
                tagBody.classList.remove('hidden');
            }
        };
        modalUserDelete.onmousedown = function (e) {
            let target = e.target;
            let modalContent = modalUserDelete.getElementsByClassName('modal__content')[0];
            if (e.target.closest('.' + modalContent.className) === null) {
                this.classList.remove('modal_active');
                tagBody.classList.remove('hidden');
            }
        };

        //вызов модалки редактирования пользователя
        let buttonOpenModalUserEdit = document.getElementById(`callback-button-user-edit-${i}`);

         buttonOpenModalUserEdit.onclick = function (e) {
             e.preventDefault();
             modalUserEdit.classList.add('modal_active');
        }

        //вызов модалки удаления пользователя
        let buttonOpenModalUserDelete= document.getElementById(`callback-button-user-delete-${i}`);

        buttonOpenModalUserDelete.onclick = function (e) {
            e.preventDefault();
            modalUserDelete.classList.add('modal_active');
        }

    }


    //страница c редактированием текущего пользователя системы
    if (document.getElementById(`modal-user-edit`) != null) {
        //модалка редактирования пользователя
        let modalUserEdit = document.getElementById(`modal-user-edit`);

        //список с модальными окнами
        let modals = new Set();
        modals.add(document.getElementById(`modal-user-edit`));

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

        modalUserEdit.onmousedown = function (e) {
            let target = e.target;
            let modalContent = modalUserEdit.getElementsByClassName('modal__content')[0];
            if (e.target.closest('.' + modalContent.className) === null) {
                this.classList.remove('modal_active');
                tagBody.classList.remove('hidden');
            }
        };

        //вызов модалки редактирования пользователя
        let buttonOpenModalUserEdit = document.getElementById(`callback-button-user-edit`);

        buttonOpenModalUserEdit.onclick = function (e) {
            e.preventDefault();
            modalUserEdit.classList.add('modal_active');
        }
    }
        
 

});