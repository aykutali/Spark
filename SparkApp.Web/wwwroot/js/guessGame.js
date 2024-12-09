const apiUrl = 'https://localhost:7005/api/GuessGame'

const selectElement = document.getElementById('gameNameSelect')
const guessButton = document.getElementById('guessButton')
const elementList = document.getElementById('gameInfo')
const guessTable = document.getElementById('guessList')
const correctGuessDiv = document.getElementById('correctGuess')

var match = window.matchMedia("(max-width: 500px)")
function myFunction(match) {
    if (match.matches) { // If media query matches
        guessTable.style.fontSize = '6px';
    }
    else {
        guessTable.style.fontSize = '16px';
    }
};

myFunction(match);

match.addEventListener('change', function () {
    myFunction(match);
});


guessButton.addEventListener('click', async () => {
    var gameName = selectElement.value;
    var isGuessIsCorrect = false
    var gameTitle = ''

    await fetch(`${apiUrl}?name=${gameName}`)
        .then(response => response.json())
        .then(game => {

            if (game.status === 400) {
                return
            }

            var tableRow = document.createElement('tr')

            var imageBox = document.createElement('th')
            var imageElement = document.createElement('img')
            imageElement.src = game.imageUrl
            imageElement.classList.add('card-img', 'img-fluid')
            imageElement.style.maxHeight = '100px'
            imageElement.style.maxWidth = '100px'
            imageElement.style.objectFit = 'contain'

            imageBox.appendChild(imageElement)
            tableRow.appendChild(imageBox)

            //------------------------------------------------

            var dateBox = document.createElement('th')
            var dateElement = document.createElement('p')

            for (let dateText in game.releaseDate) {
                dateElement.textContent = dateText

                if (game.releaseDate[dateText] === '+') {
                    dateBox.style.backgroundColor = 'green'
                }
                else if (game.releaseDate[dateText] === '-') {
                    dateBox.textContent = `<=${dateBox.textContent}`
                    dateBox.style.backgroundColor = 'red'
                }
                else if (game.releaseDate[dateText] === '*') {
                    dateBox.textContent = `${dateBox.textContent}=>`
                    dateBox.style.backgroundColor = 'red'
                }
            }

            dateBox.appendChild(dateElement)
            tableRow.appendChild(dateBox)

            //------------------------------------------------

            var devBox = document.createElement('th')
            var devElement = document.createElement('p')

            for (let devText in game.developer) {
                devElement.textContent = devText

                if (game.developer[devText] === '+') {
                    devBox.style.backgroundColor = 'green'
                }
                else {
                    devBox.style.backgroundColor = 'red'
                }
            }

            devBox.appendChild(devElement)
            tableRow.appendChild(devBox)

            //------------------------------------------------

            var directorBox = document.createElement('th')
            var directorElement = document.createElement('p')

            for (let dirText in game.leadGameDirector) {
                directorElement.textContent = dirText

                if (game.leadGameDirector[dirText] === '+') {
                    directorBox.style.backgroundColor = 'green'
                }
                else {
                    directorBox.style.backgroundColor = 'red'
                }
            }

            directorBox.appendChild(directorElement)
            tableRow.appendChild(directorBox)

            //------------------------------------------------

            var mgenreBox = document.createElement('th')
            var mgenreElement = document.createElement('p')

            for (let mgenreText in game.mainGenre) {
                mgenreElement.textContent = mgenreText

                if (game.mainGenre[mgenreText] === '+') {
                    mgenreBox.style.backgroundColor = 'green'
                }
                else {
                    mgenreBox.style.backgroundColor = 'red'
                }
            }

            mgenreBox.appendChild(mgenreElement)
            tableRow.appendChild(mgenreBox)

            //------------------------------------------------

            var sgenresBox = document.createElement('th')
            var sgElement = document.createElement('p')

            for (let sgText in game.subGenres) {

                sgElement.textContent = sgText.split(', ').join(' ')

                if (game.subGenres[sgText] === '+') {
                    sgenresBox.style.backgroundColor = 'green'
                }
                else if (game.subGenres[sgText] === '*') {
                    sgenresBox.style.backgroundColor = 'yellow'
                }
                else {
                    sgenresBox.style.backgroundColor = 'red'
                }
            }

            sgenresBox.appendChild(sgElement)
            tableRow.appendChild(sgenresBox)

            //------------------------------------------------

            var platformsBox = document.createElement('th')
            var platformsElement = document.createElement('p')

            for (let pText in game.platforms) {
                platformsElement.textContent = pText.split(', ').join(' ')

                if (game.platforms[pText] === '+') {
                    platformsBox.style.backgroundColor = 'green'
                }
                else if (game.platforms[pText] === '*') {
                    platformsBox.style.backgroundColor = 'yellow'
                }
                else {
                    platformsBox.style.backgroundColor = 'red'
                }
            }

            platformsBox.appendChild(platformsElement)
            tableRow.appendChild(platformsBox)

            //------------------------------------------------

            elementList.prepend(tableRow)

            if (game.isGuessIsCorrect === true) {
                for (let gametitle in game.title) {
                    gameTitle = gametitle
                }
                isGuessIsCorrect = true
            }
        })

    if (isGuessIsCorrect === true) {
        guessButton.style.display = 'none'
        selectElement.style.display = 'none'

        var congMessage = document.createElement('h3')
        congMessage.textContent = `Congurations your ${gameTitle} guess was right`

        var goToGameButton = document.createElement('a')
        goToGameButton.textContent = 'Go To Game'
        goToGameButton.href = `/Game/Details/${gameTitle}`

        goToGameButton.classList.add('btn', 'btn-success', 'btn-dark-text', 'btn-rounded', 'mb-2', 'w-20', 'p-3', 'fw-bold')

        correctGuessDiv.appendChild(congMessage)
        correctGuessDiv.appendChild(goToGameButton)
    }

})




