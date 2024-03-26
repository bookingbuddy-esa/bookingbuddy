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

WebUI.setText(findTestObject('Object Repository/Grupos/Page_Booking Buddy/input_Bem-vindo ao Booking Buddy_email'), 'bookingbuddy.landlord@bookingbuddy.com')

WebUI.setEncryptedText(findTestObject('Object Repository/Grupos/Page_Booking Buddy/input_O email  invlido_password'), 'CecxTDGnl/aOPUEZOhmOjg==')

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Iniciar sesso'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/i_Favoritos_bi bi-list'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/a_Grupos'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/i_Criar um grupo de reserva_bi bi-person-fill-add'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Prximo'))

WebUI.setText(findTestObject('Object Repository/Grupos/Page_Booking Buddy/input_Preencha o seguinte campo com a infor_4e208c'), 
    'grupo de teste')

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Prximo'))

WebUI.setText(findTestObject('Object Repository/Grupos/Page_Booking Buddy/input_Preencher informaes sobre o grupo_members'), 
    'testebb123@gmail.com')

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Finalizar'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/div_grupo de teste'))

WebUI.setText(findTestObject('Object Repository/Grupos/Page_Booking Buddy/input_Chat com o grupo_form-control ng-unto_2eeb6a'), 
    'mensagem de teste')

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Enviar'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/img'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/img_Logout_w-100 h-100 rounded-3'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Adicionar a Grupo de Reserva'))

WebUI.waitForElementPresent(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Lista de Grupos'), 0)

WebUI.waitForElementVisible(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Lista de Grupos'), 0)

WebUI.waitForElementPresent(findTestObject('Object Repository/Grupos/Page_Booking Buddy/div_grupo de teste1'), 0)

WebUI.waitForElementVisible(findTestObject('Object Repository/Grupos/Page_Booking Buddy/div_grupo de teste1'), 0)

WebUI.waitForElementPresent(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Adicionar'), 0)

WebUI.waitForElementVisible(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Adicionar'), 0)

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/div_grupo de teste1'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Adicionar'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/i_Favoritos_bi bi-list'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/a_Grupos'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/div_grupo de teste'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Grupo de Reserva_mat-mdc-tooltip-tri_efe8a1'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Casa em Lisboa_mat-mdc-tooltip-trigg_1c32f2'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Grupo de Reserva_mat-mdc-tooltip-tri_b369d8'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span__mat-mdc-button-touch-target'))

WebUI.waitForElementPresent(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_MAR. DE 2024'), 0)

WebUI.waitForElementVisible(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_MAR. DE 2024'), 0)

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_MAR. DE 2024'))

WebUI.waitForElementPresent(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_2025'), 0)

WebUI.waitForElementVisible(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_2025'), 0)

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_2025'))

WebUI.rightClick(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_JUN'))

WebUI.waitForElementPresent(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_MAI'), 0)

WebUI.waitForElementVisible(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_MAI'), 0)

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_MAI'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_4'))

WebUI.rightClick(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_6'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/span_6'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Reservar'))

WebUI.setText(findTestObject('Object Repository/Grupos/Page_Booking Buddy/input_Escolha o mtodo de pagamento_nif'), '274485589')

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Efetuar pagamento'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Confirmar Pagamento (DEV)'))

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Grupo de Reserva_mat-mdc-tooltip-tri_213423'))

WebUI.waitForElementPresent(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Apagar'), 0)

WebUI.waitForElementVisible(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Apagar'), 0)

WebUI.click(findTestObject('Object Repository/Grupos/Page_Booking Buddy/button_Apagar'))

WebUI.closeBrowser()

