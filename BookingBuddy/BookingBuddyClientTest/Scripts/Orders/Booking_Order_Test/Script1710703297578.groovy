import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import static com.kms.katalon.core.testobject.ObjectRepository.findWindowsObject
import com.kms.katalon.core.checkpoint.Checkpoint as Checkpoint
import com.kms.katalon.core.cucumber.keyword.CucumberBuiltinKeywords as CucumberKW
import com.kms.katalon.core.mobile.keyword.MobileBuiltInKeywords as Mobile
import com.kms.katalon.core.model.FailureHandling as FailureHandling
import com.kms.katalon.core.testcase.TestCase as TestCase
import com.kms.katalon.core.testdata.TestData as TestData
import com.kms.katalon.core.testng.keyword.TestNGBuiltinKeywords as TestNGKW
import com.kms.katalon.core.testobject.TestObject as TestObject
import com.kms.katalon.core.util.KeywordUtil as KeywordUtil
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows
import internal.GlobalVariable as GlobalVariable
import org.openqa.selenium.Keys as Keys

WebUI.openBrowser('')

WebUI.navigateToUrl('https://localhost:4200/signin')

WebUI.setText(findTestObject('Object Repository/Page_Reserva_Auto/input_Bem-vindo ao Booking Buddy_email'), 'bookingbuddy.user@bookingbuddy.com')

WebUI.setEncryptedText(findTestObject('Object Repository/Page_Reserva_Auto/input_O email  invlido_password'), 'rmPWz4Y+3TnrVr7CE0MgbA==')

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/button_Iniciar sesso'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/img_Logout_w-100 h-100 rounded-3'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/span__mat-mdc-button-touch-target'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/span_28'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/span_31'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/button_Reservar'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/button_Efetuar pagamento'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/button_Confirmar Pagamento (DEV)'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/a_aqui'))

WebUI.click(findTestObject('Object Repository/Page_Reserva_Auto/a_Propriedade 79265095'))

WebUI.waitForElementVisible(findTestObject('Object Repository/Page_Reserva_Auto/div_Propriedade 79265095Data de Check-In 28_301835'), 
    0)

