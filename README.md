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

## Cómo instalar
Dirijase a la pestaña "Releases" en este repositorio o de forma alternativa en la carpeta 'Ejecutable'. Descargue el archivo en zip y descomprimalo. Allí econtrará el EXE de la aplicación. 

## Manejo de errores
El analizador léxico reporta tokens no válidos, sin embargo, aún así intenta llevar el código al analizador léxico simplemente ignorandolos. En el analizador léxico, cada vez que se encuentra un error la gramática se reinicia de cero y regresa a buscar declaraciones de variables y funciones, además se ignora por completo la línea donde estaba el error de léxico, por lo que es posible que si hay un error dentro de una función se reporten todas las líneas subsecuentes como inválidas, hasta encontrar una nueva declaración.

## Instrucciones de uso

En un principio, se muestra una pantalla como la siguiente:
<br><a href="https://imgur.com/QOXVr3f"><img src="https://i.imgur.com/QOXVr3f.png" title="source: imgur.com" width="300" height="300"/></a>

Al seleccionar un archivo, se analizará y mostrará el resultado en pantalla:
<br><a href="https://imgur.com/URBl04J"><img src="https://i.imgur.com/URBl04J.png" title="source: imgur.com" width="600" height="300"/></a>
<br>Se pueden observar en esta parte, los diferentes errores que pudieron ocurrir en el escaneo.

Finalmente, se puede guardar ese resultado al dar click en el botón "Save":
<br><a href="https://imgur.com/nYY3onj"><img src="https://i.imgur.com/nYY3onj.png" title="source: imgur.com" width="600" height="300"/></a>
