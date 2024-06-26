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
import com.kms.katalon.core.testobject.TestObject
import com.kms.katalon.core.util.KeywordUtil
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows
import internal.GlobalVariable as GlobalVariable
import org.openqa.selenium.Keys as Keys

WebUI.openBrowser('https://localhost:4200/register')

WebUI.waitForPageLoad(30)

WebUI.setText(findTestObject('Authentication/Page_Register/input_name'), 'Booking Buddy')

WebUI.setText(findTestObject('Authentication/Page_Register/input_email'), ('bookingbuddy.bb+' + java.util.UUID.randomUUID()) + '@gmail.com')

WebUI.setEncryptedText(findTestObject('Authentication/Page_Register/input_password'), 'YEAN24mcKX30D4bwva2Buw==')

WebUI.setEncryptedText(findTestObject('Authentication/Page_Register/input_confirmPassword'), 'YEAN24mcKX30D4bwva2Buw==')

WebUI.click(findTestObject('Authentication/Page_Register/button_CreateAccount'))

boolean visible = WebUI.waitForElementVisible(findTestObject('Authentication/Page_Register/div_register_success'), 10)

if (!visible) {
	KeywordUtil.markFailed("Element 'div_register_success' was not visible within the specified timeout.")
}

WebUI.closeBrowser()