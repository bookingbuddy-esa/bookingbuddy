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
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows
import internal.GlobalVariable as GlobalVariable
import org.openqa.selenium.Keys as Keys

WebUI.openBrowser('')

WebUI.navigateToUrl('https://localhost:4200/signin')

WebUI.setText(findTestObject('Object Repository/Page_Groups/input_Bem-vindo ao Booking Buddy_email'), 
    'bookingbuddy.landlord@bookingbuddy.com')

WebUI.setEncryptedText(findTestObject('Object Repository/Page_Groups/input_O email  invlido_password'), 
    'CecxTDGnl/aOPUEZOhmOjg==')

WebUI.click(findTestObject('Object Repository/Page_Groups/button_Iniciar sesso'))

WebUI.click(findTestObject('Object Repository/Page_Groups/a_Home_navbarDarkDropdownMenuLink'))

WebUI.click(findTestObject('Object Repository/Page_Groups/a_Criar Grupo'))

WebUI.click(findTestObject('Object Repository/Page_Groups/button_Prximo'))

WebUI.setText(findTestObject('Object Repository/Page_Groups/input_Preencha o seguinte campo com a infor_4e208c'), 
    'Grupo de Viagem 1')

WebUI.click(findTestObject('Object Repository/Page_Groups/button_Prximo'))

WebUI.setText(findTestObject('Object Repository/Page_Groups/input_Preencher informaes sobre o grupo_members'), 
    '123bbteste@gmail.com')

WebUI.click(findTestObject('Object Repository/Page_Groups/button_Finalizar'))

WebUI.click(findTestObject('Object Repository/Page_Groups/div_Grupo de Viagem 1'))

WebUI.setText(findTestObject('Object Repository/Page_Groups/input_Chat com o grupo_form-control ng-unto_2eeb6a'), 
    'olá')

WebUI.click(findTestObject('Object Repository/Page_Groups/button_Enviar'))

WebUI.closeBrowser()

