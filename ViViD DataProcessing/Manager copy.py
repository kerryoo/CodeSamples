import mysql.connector
from mysql.connector import errorcode
import xlwt
from xlwt import Workbook
import constant

currLevel = positionsAdded = startingRowBuffer = 0

def main():
    try:
        connection = mysql.connector.connect(user='kerryoo',
                                             password='changedpasswordtoposttogithub',
                                             host='127.0.0.0',
                                             database='unity access')

        sql_select_Query = "select * from playerdata"
        cursor = connection.cursor()
        cursor.execute(sql_select_Query)
        mySqlTable = cursor.fetchall() #gather the data from the mySQL database
        playerData = []

        #create a list of all the entries in the database. Each entry has a level, amount of time the player used his
        #special ability in that level, time to complete the level, amount of deaths, and a file of recorded positions
        for row in mySqlTable:
            playerData.append({ constant.LEVEL : row[1], #row[0] has the username but it is unimportant
                                constant.SPECIAL_COUNT : row[2],
                                constant.COMPLETION_TIME : row[3],
                                constant.DEATH_COUNT : row[4],
                                constant.POSITIONS = row[5] })

        dataWorkbook = xlwt.Workbook(encoding="utf-8")
        specialCountSheet = dataWorkbook.add_sheet(constant.SPECIAL_SHEET)
        completionTimeSheet = dataWorkbook.add_sheet(constant.COMPLETION_SHEET)
        deathCountSheet = dataWorkbook.add_sheet(constant.DEATH_SHEET)
        positionsSheet = dataWorkbook.add_sheet(constant.POSITION_SHEET)

        for row, entry in enumerate(playerData):
            global currLevel = entry[constant.LEVEL]
            addSpecialCountEntry(specialCountSheet, row, entry[constant.SPECIAL_COUNT])
            addCompletionTimeEntry(completionTimeSheet, row, entry[constant.COMPLETION_TIME])
            addDeathCountEntry(deathCountSheet, row, entry[constant.DEATH_COUNT])
            addPositionEntries(positionsSheet, row, entry[constant.POSITIONS])

        dataWorkbook.save('VividUserData.xls')
        print('Workbook successfully populated and saved')

    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            print("Invalid username or password.")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            print("Database does not exist")
        else:
            print(err)

    finally:
        if connection.is_connected():
            connection.close()
            cursor.close()
            print("MySQL connection sucessfully closed.")

def addSpecialCountEntry(specialCountSheet, row, specialCount):
    specialCountSheet.write(row, constant.LEVEL_COLUMN, currLevel)
    specialCountSheet.write(row, constant.SPECIAL_COUNT_COLUMN, specialCount)

def addCompletionTimeEntry(completionTimeSheet, row, completionTime):
    completionTimeSheet.write(row, constant.LEVEL_COLUMN, currLevel)
    completionTimeSheet.write(row, constant.COMPLETION_TIME_COLUMN, completionTime)

def addDeathCountEntry(deathCountSheet, row, deathCount):
    deathCountSheet.write(row, constant.LEVEL_COLUMN, currLevel)
    deathCountSheet.write(row, constant.DEATH_COUNT_COLUMN, deathCount)

def addPositionEntries(positionsSheet, positionFile):
    global positionsAdded
    global startingRowBuffer
    row = positionsAdded + startingRowBuffer
    positionEntries = positionFile.split('\n')

    for positionEntry in positionEntries:
        entryValues = positionEntry.split('\t')
        positionsSheet.write(row, constant.LEVEL_COLUMN, entryValues[constant.LEVEL_INDEX])
        positionsSheet.write(row, constant.TIME_COLUMN, entryValues[constant.TIME_INDEX])
        positionsSheet.write(row, constant.POSITION_X_COLUMN, entryValues[constant.X_INDEX])
        positionsSheet.write(row, constant.POSITION_Y_COLUMN, entryValues[constant.Y_INDEX])
        positionsSheet.write(row, constant.POSITION_Z_COLUMN, entryValues[constant.Z_INDEX])

        row += 1
        positionsAdded += 1

    startingRowBuffer += 1

