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

WebUI.setText(findTestObject('Authentication/Page_Login/input_email'), 'bookingbuddy.user@bookingbuddy.com')

WebUI.setEncryptedText(findTestObject('Authentication/Page_Login/input_password'), 'rmPWz4Y+3TnrVr7CE0MgbA==')

WebUI.click(findTestObject('Authentication/Page_Login/button_SignIn'))

boolean visible = WebUI.waitForElementVisible(findTestObject('Page_Home/div_firstProperty'), 30)

if (visible) {
    WebUI.click(findTestObject('Page_Home/img_firstProperty'))

    WebUI.verifyElementVisible(findTestObject('Page_PropertyAd/div_propertyInfo'))

    String value = WebUI.getAttribute(findTestObject('Page_PropertyAd/button_Favorite'), 'title')
	
	if(value.equals('Adicionar aos Favoritos')) {
		WebUI.click(findTestObject('Page_PropertyAd/button_Favorite'))
		
		WebUI.verifyElementAttributeValue(findTestObject('Page_PropertyAd/button_Favorite'), 'title', 'Remover dos Favoritos', 30)
	}else {
		WebUI.click(findTestObject('Page_PropertyAd/button_Favorite'))
		
		WebUI.verifyElementAttributeValue(findTestObject('Page_PropertyAd/button_Favorite'), 'title', 'Adicionar aos Favoritos', 30)
	}
} else {
    KeywordUtil.markWarning('No property listed!')
}

WebUI.closeBrowser()
