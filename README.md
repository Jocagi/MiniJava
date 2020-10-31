# MiniJava <img src="https://devicons.github.io/devicon/devicon.git/icons/java/java-original-wordmark.svg" alt="java" width="50" height="50"/>
Proyecto de Compiladores en Universidad Rafael Landívar

## Autores
<em>Karen Izabel Paiz - 1215718 </em><br>
<em>José Carlos Girón - 1064718 </em>

## About

### Fase 1: Analizador léxico
Se inicia el compilador con la aplicación del análisis léxico. Se crea un escáner para el lenguaje de programación 'Java'. 
El escáner reconoce los tokens propios del lenguaje en el orden en que se leen, hasta el final del archivo. Determina sus atributos para que la información sobre cada símbolo esté correctamente impresa.
### Fase 2 (Laboratorio): Analizador sintáctico descendente recursivo
En la segunda fase del proceso, se valida la posición correcta de los tokens según la gramática definida para el lenguaje. 
### Fase 2: Analizador sintáctico ascendente
Se valida la posición correcta de los tokens según la gramática definida para el lenguaje. En este caso, se ha utilizado el método LR(1) para obtener la tabla de análisis sintáctico con la cual se realiza el parseo. 

## Cómo instalar
Dirijase a <a href="https://github.com/Jocagi/MiniJava/releases">"Releases"</a> en este repositorio o de forma alternativa en la carpeta 'Ejecutable'. Descargue el archivo en zip y descomprimalo. Allí econtrará el EXE de la aplicación. 

## Manejo de errores
El analizador léxico reporta tokens no válidos, sin embargo, aún así intenta llevar el código al analizador sintáctico simplemente ignorandolos. En el analizador sintáctico, cada vez que se encuentra un error la gramática se reinicia de cero y regresa a buscar declaraciones de variables, constantes, interfaces, funciones y clases, además se ignora por completo la línea donde estaba el error de léxico, por lo que es posible que si hay un error dentro de una función se reporten todas las líneas subsecuentes como inválidas, hasta encontrar una nueva declaración.

## Instrucciones de uso

En un principio, se muestra una pantalla como la siguiente:
<br><a href="https://imgur.com/sRUal6C"><img src="https://i.imgur.com/sRUal6C.png" title="source: imgur.com" width="300" height="300"/></a><br>

Es posible ver la tabla de analisis sintactico si se presiona el botón "Mostrar Tabla". Este proceso puede durar unos minutos.
<br><a href="https://imgur.com/jiEz5EO"><img src="https://i.imgur.com/jiEz5EO.png" title="source: imgur.com" width="600" height="300"/></a><br>

Para realizar el analisis de debe seleccionar un archivo:
<br><a href="https://imgur.com/5z4ZvU1"><img src="https://i.imgur.com/5z4ZvU1.png" title="source: imgur.com" width="300" height="300"/></a><br>

Este se analizará y mostrará el resultado en pantalla, indicando en rojo lo errores encontrados:
<br><a href="https://imgur.com/uIr49mG"><img src="https://i.imgur.com/uIr49mG.png" title="source: imgur.com" width="300" height="300"/></a><br>
