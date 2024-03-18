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

WebUI.openBrowser('https://localhost:4200/signin')

WebUI.waitForPageLoad(30)

WebUI.setText(findTestObject('Authentication/Page_Login/input_email'), 'bookingbuddy.admin@bookingbuddy.com')

WebUI.setEncryptedText(findTestObject('Authentication/Page_Login/input_password'), 'LFjcuhPuhC+N++WAjwC4kA==')

WebUI.click(findTestObject('Authentication/Page_Login/button_SignIn'))

boolean present = WebUI.waitForElementPresent(findTestObject('Page_Home/div_homepage'), 30)

if (!(present)) {
    KeywordUtil.markFailed('Element \'div_homepage\' was not present within the specified timeout.')
}

WebUI.navigateToUrl('https://localhost:4200/hosting/create')

WebUI.waitForPageLoad(30)

WebUI.click(findTestObject('Hosting/PropertyAdCreate/button_Next'))

WebUI.setText(findTestObject('Hosting/PropertyAdCreate/input_location'), 'R. de Santa Catarina, 4000 Porto, Portugal')

WebUI.waitForElementClickable(findTestObject('Hosting/PropertyAdCreate/button_Next'), 30)

WebUI.click(findTestObject('Hosting/PropertyAdCreate/button_Next'))

WebUI.click(findTestObject('Hosting/PropertyAdCreate/button_Next'))

WebUI.click(findTestObject('Hosting/PropertyAdCreate/button_Next'))

int propertyId = new Random().nextInt(1,10000)

WebUI.setText(findTestObject('Hosting/PropertyAdCreate/input_name'), 'Propriedade ' + propertyId)

WebUI.setText(findTestObject('Hosting/PropertyAdCreate/textarea_description'), 'Descrição detalhada da Propriedade ' + propertyId)

WebUI.setText(findTestObject('Hosting/PropertyAdCreate/input_price'), new Random().nextInt(0,1000).toString())

WebUI.click(findTestObject('Hosting/PropertyAdCreate/button_Finish'))

present = WebUI.waitForElementPresent(findTestObject('Page_Home/div_homepage'), 30)

if (!(present)) {
	KeywordUtil.markFailed('Element \'div_homepage\' was not present within the specified timeout.')
}

WebUI.closeBrowser()

